using bot.service.manager.Model;
using bot.service.manager.Model.KubeService;
using Newtonsoft.Json;

namespace bot.service.manager.Service
{
    public class KubeFileConverter
    {
        private readonly ILogger<KubeFileConverter> _logger;

        public KubeFileConverter(ILogger<KubeFileConverter> logger)
        {
            _logger = logger;
        }

        public async Task<PodRootModel> GetPodInstance(string result)
        {
            PodRootModel podRootModel = JsonConvert.DeserializeObject<PodRootModel>(result);
            if (podRootModel != null)
            {
                _logger.LogInformation($"[SUCCESS]: {podRootModel!.kind}");
            }
            else
            {
                _logger.LogError($"[NOT FOUND]: Pod not found");
            }

            _logger.LogInformation($"[INFO]: Command execution completed");

            return await Task.FromResult(podRootModel);
        }

        public async Task<ServiceRootModel> GetServiceInstance(string result)
        {
            ServiceRootModel serviceRootModel = JsonConvert.DeserializeObject<ServiceRootModel>(result);
            if (serviceRootModel != null)
            {
                _logger.LogInformation($"[SUCCESS]: {serviceRootModel!.kind}");
            }
            else
            {
                _logger.LogError($"[NOT FOUND]: Service not found");
            }

            _logger.LogInformation($"[INFO]: Command execution completed");

            return await Task.FromResult(serviceRootModel);
        }
    }
}
