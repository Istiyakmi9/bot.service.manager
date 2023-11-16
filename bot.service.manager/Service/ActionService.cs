using bot.service.manager.IService;
using bot.service.manager.Model;
using System.Diagnostics;

namespace bot.service.manager.Service
{
    public class ActionService: IActionService
    {
        public async Task<string> CheckStatusService(KubectlModel kubectlModel)
        {
            string result = RunMicrok8sKubectlCommand(kubectlModel);
            return await Task.FromResult(result);
        }


        private string RunMicrok8sKubectlCommand(KubectlModel kubectlModel)
        {
            string arguments = string.Empty;

            if (string.IsNullOrEmpty(kubectlModel.Command))
            {
                kubectlModel.Command = "microk8s.kubectl get pods all --all-namespaces";
            }


            // Create process start info
            var startInfo = new ProcessStartInfo
            {
                FileName = "sh",
                Arguments = "-c microk8s.kubectl get pods all --all-namespaces",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Start the process
            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();

                // Capture the standard output
                string result = process.StandardOutput.ReadToEnd();

                // Wait for the process to exit
                process.WaitForExit();

                return result;
            }
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
