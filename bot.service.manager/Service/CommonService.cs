using bot.service.manager.Model;
using bot.service.manager.Model.KubeService;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Renci.SshNet;
using System.Diagnostics;

namespace bot.service.manager.Service
{
    public class CommonService
    {
        private readonly ILogger<CommonService> _logger;
        private readonly KubeFileConverter _kubeFileConverter;
        private readonly IConfiguration _configuration;
        private readonly RemoteServerConfig _remoteServerConfig;

        public CommonService(
            ILogger<CommonService> logger,
            KubeFileConverter kubeFileConverter,
            IOptions<RemoteServerConfig> options)
        {
            _logger = logger;
            _kubeFileConverter = kubeFileConverter;
            _remoteServerConfig = options.Value;
        }

        public async Task<string> ExecutedCommandInWindows(KubectlModel kubectlModel)
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

        public async Task<string> ExecutedCommandInLinux(KubectlModel kubectlModel)
        {
            string host = _remoteServerConfig.host;
            string username = _remoteServerConfig.username;
            string password = _remoteServerConfig.password;

            string result = string.Empty;

            using (var client = new SshClient(host, username, password))
            {
                client.Connect();

                // Command to execute kubectl remotely
                string kubectlCommand = $"/snap/bin/microk8s.kubectl {kubectlModel.Command}";

                // Run the kubectl command
                var command = client.RunCommand(kubectlCommand);
                result = command.Result;

                client.Disconnect();
            }

            return await Task.FromResult(result);
        }

        public async Task<string> RunAllCommandService(KubectlModel kubectlModel)
        {
            string result = string.Empty;
            switch (_remoteServerConfig.env)
            {
                case "development":
                    result = await ExecutedCommandInWindows(kubectlModel);
                    break;
                case "staging":
                    result = await ExecutedCommandInLinux(kubectlModel);
                    break;
            }

            return result;
        }


        public async Task<PodRootModel> RunCommandForPodService(KubectlModel kubectlModel)
        {
            PodRootModel podRootModel = null;

            try
            {
                string result = await RunAllCommandService(kubectlModel);
                if (string.IsNullOrEmpty(result))
                {
                    throw new Exception("Fail to convert pod json to object");
                }

                podRootModel = await _kubeFileConverter.GetPodInstance(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ERROR]: {ex.Message}");
            }

            return await Task.FromResult(podRootModel);
        }

        public async Task<ServiceRootModel> RunCommandToJsonService(KubectlModel kubectlModel, string fileType)
        {
            ServiceRootModel serviceRootModel = null;

            try
            {
                string result = await RunAllCommandService(kubectlModel);
                if (string.IsNullOrEmpty(result))
                {
                    throw new Exception("Fail to convert service json to object");
                }

                serviceRootModel = await _kubeFileConverter.GetServiceInstance(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ERROR]: {ex.Message}");
            }

            return await Task.FromResult(serviceRootModel);
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
