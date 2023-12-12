using bot.service.manager.IService;
using bot.service.manager.Model;

namespace bot.service.manager.Service
{
    public class ActionService : IActionService
    {
        ILogger<ActionService> _logger;
        private readonly CommonService _commonService;
        private readonly YamlUtilService _yamlUtilService;

        public ActionService(ILogger<ActionService> logger, CommonService commonService, YamlUtilService yamlUtilService)
        {
            _logger = logger;
            _commonService = commonService;
            _yamlUtilService = yamlUtilService;
        }

        public async Task<GitHubContent> CheckStatusService(GitHubContent gitHubContent)
        {
            return await _commonService.FindServiceStatus("api-databuilder-service");
        }

        public async Task<GitHubContent> ReRunFileService(GitHubContent gitHubContent)
        {
            if (string.IsNullOrEmpty(gitHubContent.DownloadUrl))
                throw new Exception("Invalid url");

            var result = await StopFileService(gitHubContent);
            await RunFileService(gitHubContent);
            return result;
        }

        public async Task<GitHubContent> RunFileService(GitHubContent gitHubContent)
        {
            if (string.IsNullOrEmpty(gitHubContent.DownloadUrl))
                throw new Exception("Invalid url");

            string _namespace = "default";
            YamlModel yamlModel = await _yamlUtilService.GetGithubYamlFile(gitHubContent.DownloadUrl);
            if (yamlModel.Metadata != null && !string.IsNullOrEmpty(yamlModel.Metadata.Namespace))
            {
                _namespace = yamlModel.Metadata.Namespace;
            }

            KubectlModel kubectlModel = new KubectlModel
            {
                IsMicroK8 = true,
                IsWindow = false,
                Command = $"apply -f {gitHubContent.DownloadUrl} -n {_namespace}"
            };

            string result = await _commonService.RunAllCommandService(kubectlModel);

            gitHubContent.Status = false;
            if (!string.IsNullOrEmpty(result) && result.ToLower().Contains("created"))
                gitHubContent.Status = true;
            else
                throw new Exception(result);

            return gitHubContent;
        }

        public async Task<GitHubContent> StopFileService(GitHubContent gitHubContent)
        {
            if (string.IsNullOrEmpty(gitHubContent.DownloadUrl))
                throw new Exception("Invalid url");

            KubectlModel kubectlModel = new KubectlModel
            {
                IsMicroK8 = true,
                IsWindow = false,
                Command = $"delete -f {gitHubContent.DownloadUrl}"
            };

            var result = await _commonService.RunAllCommandService(kubectlModel);

            gitHubContent.Status = true;
            if (!string.IsNullOrEmpty(result) && result.ToLower().Contains("deleted"))
                gitHubContent.Status = false;
            else
                throw new Exception(result);

            return gitHubContent;
        }

        public async Task<string> GetAllRunningService()
        {
            var status = "";
            KubectlModel kubectlModel = new KubectlModel
            {
                IsMicroK8 = true,
                IsWindow = false,
                Command = "get pods"
            };
            var result = await _commonService.RunAllCommandService(kubectlModel);
            return await Task.FromResult(status);
        }
    }

    public enum FileType
    {
        DEPLOYMENT,
        SERVICE,
        PERSISTENTVOLUME,
        PERSISTENTVOLUMECLAIM,
        CONFIGMAP,
        INGRESS,
        CLUSTERISSUER,
        STATEFULSET,
        NAMESPACE
    }
}
