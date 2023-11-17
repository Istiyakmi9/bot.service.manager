using bot.service.manager.IService;
using bot.service.manager.Model;
using System.Diagnostics;

namespace bot.service.manager.Service
{
    public class ActionService : IActionService
    {
        ILogger<ActionService> _logger;

        public ActionService(ILogger<ActionService> logger)
        {
            _logger = logger;
        }

        public async Task<string> CheckStatusService(KubectlModel kubectlModel)
        {
            string result = RunMicrok8sKubectlCommand(kubectlModel);
            return await Task.FromResult(result);
        }


        private string RunMicrok8sKubectlCommand(KubectlModel kubectlModel)
        {
            string arguments = string.Empty;

            // Set the path to microk8s kubectl executable
            string microk8sKubectlPath = "/snap/bin/microk8s.kubectl"; // Adjust the path based on your setup

            kubectlModel.Command = "microk8s.kubectl get pods all --all-namespaces";

            _logger.LogInformation($"[CMD]: {kubectlModel.Command}");

            string result = string.Empty;

            try
            {
                // Create process start info
                var startInfo = new ProcessStartInfo
                {
                    FileName = microk8sKubectlPath,
                    Arguments = kubectlModel.Command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                _logger.LogInformation($"[INFO]: {kubectlModel.Command}");
                // Start the process
                using (Process process = new Process { StartInfo = startInfo })
                {
                    _logger.LogInformation($"[INFO]: Starting command execution");
                    process.Start();

                    // Capture the standard output
                    result = process.StandardOutput.ReadToEnd();
                    _logger.LogInformation($"[RESULT]: {result}");

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
            return result;
        }

        public async Task<string> ReRunFileService(FileDetail fileDetail)
        {
            return await Task.FromResult("Successfull");
        }

        public async Task<string> RunFileService(FileDetail fileDetail)
        {
            return await Task.FromResult("Successfull");
        }

        public async Task<string> StopFileService(FileDetail fileDetail)
        {
            return await Task.FromResult("Successfull");
        }
    }
}
