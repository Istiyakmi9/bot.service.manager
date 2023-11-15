using Core.Pipeline.IService;
using Core.Pipeline.Model;
using System.Diagnostics;

namespace Core.Pipeline.Service
{
    public class FolderDiscoveryService : IFolderDiscoveryService
    {
        public async Task<FolderDiscovery> GetFolderDetailService(string targetDirectory)
        {
            if (string.IsNullOrEmpty(targetDirectory))
                targetDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "workspace"));

            if (!Directory.Exists(targetDirectory))
                throw new Exception("Invalid directory");

            var result = GetAllFilesInDirectory(targetDirectory);
            result.RootDirectory = Directory.GetCurrentDirectory();

            return await Task.FromResult(result);
        }

        private FolderDiscovery GetAllFilesInDirectory(string targetDirectory)
        {
            FolderDiscovery folderDiscovery = new FolderDiscovery();
            folderDiscovery.Files = new List<FileDetail>();
            var list = new List<string>();
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            foreach (string fileName in fileEntries)
            {
                string extension = Path.GetExtension(fileName);
                if (extension.Equals(".yml") || extension.Equals(".yaml"))
                {
                    folderDiscovery.Files.Add(new FileDetail
                    {
                        FullPath = fileName,
                        FileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1),
                        FolderName = targetDirectory
                    });
                }
                list.Add(fileName);
            }
            folderDiscovery.Folders = Directory.GetDirectories(targetDirectory).ToList();

            return folderDiscovery;
        }

        public async Task<string> RunCommandService()
        {
            string result = string.Empty;
            // Specify the command to run
            string command = "dir"; // Replace this with your desired command

            // Create a new process start info
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe", // Specify the command prompt, "/bin/bash" Specify the bash shell for Linux
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "/c " + command // /c tells cmd.exe to terminate after the command is complete ---- $"-c \"{command}\"" -c tells bash to execute the command for Linux
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
