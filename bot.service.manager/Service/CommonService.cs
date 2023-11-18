using bot.service.manager.Model;
using System.Diagnostics;

namespace bot.service.manager.Service
{
    public class CommonService
    {
        private readonly ILogger<CommonService> _logger;

        public CommonService(ILogger<CommonService> logger)
        {
            _logger = logger;
        }

        public async Task<string> RunAllCommandService(KubectlModel kubectlModel)
        {
            string result = string.Empty;
            string cmdPrefix = string.Empty;
            string arguments = kubectlModel.Command;

            if (kubectlModel.IsMicroK8)
            {
                cmdPrefix = "/snap/bin/microk8s.kubectl";
            }
            else
            {
                if (kubectlModel.IsWindow)
                {
                    cmdPrefix = "cmd.exe";
                    arguments = "/c " + kubectlModel.Command;
                }
                else
                {
                    cmdPrefix = "/bin/bash";
                }
            }

            _logger.LogInformation($"[CMD]: {kubectlModel.Command}");
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
                    _logger.LogInformation($"[INFO]: Starting command execution");
                    process.Start();

                    // Read the output and error streams
                    result = process.StandardOutput.ReadToEnd();
                    _logger.LogInformation($"[RESULT]: {result}");
                    if (string.IsNullOrEmpty(result))
                    {
                        string error = process.StandardError.ReadToEnd();
                        Console.WriteLine("Error:\n" + error);
                        throw new Exception(error);
                    }

                    // Wait for the process to exit
                    process.WaitForExit();

                    _logger.LogInformation($"[INFO]: Command execution completed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ERROR]: {ex.Message}");
            }

            _logger.LogInformation($"[RESULT] Final: {result}");
            return await Task.FromResult(result);
        }

        public async Task<FileDetail> FindServiceStatus(string serviceName, string nameSpaces = "default")
        {
            string options = "'{.status.loadBalancer.ingress[0].ip}{.spec.clusterIP}'";
            KubectlModel kubectlModel = new KubectlModel
            {
                Command = $"get service {serviceName} -n {nameSpaces} -o jsonpath={options}",
                IsMicroK8 = true,
            };

            var status = await RunAllCommandService(kubectlModel);
            return new FileDetail { Status = string.IsNullOrEmpty(status) ? false : true };
        }
    }
}
