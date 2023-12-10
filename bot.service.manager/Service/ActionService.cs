using bot.service.manager.IService;
using bot.service.manager.Model;
using System.Diagnostics;

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

        public async Task<FileDetail> ReRunFileService(FileDetail fileDetail)
        {
            return await Task.FromResult(new FileDetail());
        }

        public async Task<FileDetail> RunFileService(FileDetail fileDetail)
        {
            if (string.IsNullOrEmpty(fileDetail.FullPath))
                throw new Exception("Invalid file path");

            KubectlModel kubectlModel = new KubectlModel
            {
                IsMicroK8 = true,
                IsWindow = false,
                Command = $"apply -f {fileDetail.FullPath}"
            };

            string result = await _commonService.RunAllCommandService(kubectlModel);

            fileDetail.Status = false;
            if (!string.IsNullOrEmpty(result) && result.ToLower().Contains("created"))
            {
                fileDetail.Status = true;
            }
            else
            {
                throw new Exception(result);
            }

            return fileDetail;
        }

        public async Task<FileDetail> StopFileService(FileDetail fileDetail)
        {
            if (string.IsNullOrEmpty(fileDetail.FullPath))
                throw new Exception("Invalid file path");

            KubectlModel kubectlModel = new KubectlModel
            {
                IsMicroK8 = true,
                IsWindow = false,
                Command = $"delete -f {fileDetail.FullPath}"
            };

            var result = await _commonService.RunAllCommandService(kubectlModel);

            fileDetail.Status = true;
            if (!string.IsNullOrEmpty(result) && result.ToLower().Contains("deleted"))
            {
                fileDetail.Status = false;
            }
            else
            {
                throw new Exception(result);
            }

            return fileDetail;
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
