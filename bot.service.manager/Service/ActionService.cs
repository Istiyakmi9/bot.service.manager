using bot.service.manager.IService;
using bot.service.manager.Model;

namespace bot.service.manager.Service
{
    public class ActionService : IActionService
    {
        ILogger<ActionService> _logger;
        private readonly CommonService _commonService;

        public ActionService(ILogger<ActionService> logger, CommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        public async Task<FileDetail> CheckStatusService(KubectlModel kubectlModel)
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

            KubectlModel kubectlModel = new KubectlModel
            {
                IsMicroK8 = true,
                IsWindow = false,
                Command = $"apply -f {gitHubContent.DownloadUrl}"
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
