using bot.service.manager.IService;
using bot.service.manager.Model;
using System.Diagnostics;

namespace bot.service.manager.Service
{
    public class FolderDiscoveryService : IFolderDiscoveryService
    {
        public async Task<FolderDiscovery> GetFolderDetailService(string targetDirectory)
        {
            if (string.IsNullOrEmpty(targetDirectory))
                targetDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "k8-workspace"));

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            var result = GetAllFilesInDirectory(targetDirectory);
            result.RootDirectory = Directory.GetCurrentDirectory();
            return await Task.FromResult(result);
        }

        private FolderDiscovery GetAllFilesInDirectory(string targetDirectory)
        {
            FolderDiscovery folderDiscovery = new FolderDiscovery();
            folderDiscovery.Files = new List<FileDetail>();
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            foreach (string fileName in fileEntries)
            {
                string extension = Path.GetExtension(fileName);
                if (extension.Equals(".yml") || extension.Equals(".yaml"))
                {
                    folderDiscovery.Files.Add(new FileDetail
                    {
                        FullPath = fileName,
                        FileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1)
                    });
                }
            }
            folderDiscovery.FolderPath = targetDirectory;
            folderDiscovery.FolderName = targetDirectory.Split(@"\").Last();
            return folderDiscovery;
        }

        public async Task<string> RunCommandService(KubectlModel kubectlModel)
        {
            string result = string.Empty;
            string cmdPrefix = string.Empty;
            string arguments = $"-c \"{kubectlModel.Command}\"";
            if (kubectlModel.isMicroK8)
            {
                cmdPrefix = "/snap/bin/microk8.kubectl ";
            } else
            {
                if (kubectlModel.isWindow)
                {
                    cmdPrefix = "cmd.exe";
                    arguments = "/c " + kubectlModel.Command;
                }
                else
                    cmdPrefix = "/bin/bash";
            }
            // Create a new process start info
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = cmdPrefix, // Specify the command prompt, "/bin/bash" Specify the bash shell for Linux
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = arguments  // /c tells cmd.exe to terminate after the command is complete ---- $"-c \"{command}\"" -c tells bash to execute the command for Linux
            };

            // Create a new process and assign the start info
            using (Process process = new Process { StartInfo = psi })
            {
                // Start the process
                process.Start();

                // Read the output and error streams
                result = process.StandardOutput.ReadToEnd();
                if (string.IsNullOrEmpty(result))
                {
                    string error = process.StandardError.ReadToEnd();
                    Console.WriteLine("Error:\n" + error);
                    throw new Exception(error);
                }

                // Wait for the process to exit
                process.WaitForExit();
            }

            return await Task.FromResult(result);
        }
    }
}
