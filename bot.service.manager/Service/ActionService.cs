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

        public async Task<string> CheckStatusService(KubectlModel kubectlModel)
        {
            string result = await _commonService.FindServiceStatus("api-databuilder-service");
            return result;
        }


        public async Task<string> ReRunFileService(FileDetail fileDetail)
        {
            return await Task.FromResult("Successfull");
        }

        public async Task<string> RunFileService(FileDetail fileDetail)
        {
            if (string.IsNullOrEmpty(fileDetail.FullPath))
                throw new Exception("Invalid file path");

            KubectlModel kubectlModel = new KubectlModel
            {
                IsMicroK8 = true,
                IsWindow = false,
                Command = $"apply -f {fileDetail.FullPath}"
            };
            var result = await _commonService.RunAllCommandService(kubectlModel);
            return result;
        }

        public async Task<string> StopFileService(FileDetail fileDetail)
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
            return result;
        }
    }
}
