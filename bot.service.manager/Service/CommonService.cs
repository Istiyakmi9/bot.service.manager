using bot.service.manager.Model;
using System.Diagnostics;

namespace bot.service.manager.Service
{
    public class CommonService
    {
        public static async Task<string> RunAllCommandService(KubectlModel kubectlModel)
        {
            string result = string.Empty;
            string cmdPrefix = string.Empty;
            string arguments = $"-c \"{kubectlModel.Command}\"";
            if (kubectlModel.isMicroK8)
            {
                cmdPrefix = "/snap/bin/microk8.kubectl ";
            }
            else
            {
                if (kubectlModel.isWindow)
                {
                    cmdPrefix = "cmd.exe";
                    arguments = "/c " + kubectlModel.Command;
                }
                else
                    cmdPrefix = "/bin/bash";
            }
            try
            {
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
            catch (Exception)
            {
                throw;
            }

        }
    }
}
