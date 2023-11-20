using bot.service.manager.Model;

namespace bot.service.manager.Service
{
    public class PodHelper
    {
        private readonly ILogger<PodHelper> _logger;

        public PodHelper(ILogger<PodHelper> logger)
        {
            _logger = logger;
        }

        public ItemStatus FindPodStatus(PodRootModel podRootModel, string podName)
        {
            var currentPod = podRootModel.items.FirstOrDefault(x => x.metadata.name.ToLower().StartsWith(podName.ToLower()));
            _logger.LogInformation($"[POD INFO]: Pod: {podName}, Status: Checking status");

            if (currentPod == null)
            {
                _logger.LogInformation($"[POD INFO]: Pod: {podName}, Status: Not found");
                return ItemStatus.NotCreated;
            }

            _logger.LogInformation($"[POD STATUS]: Pod: {podName}, Status: {currentPod.metadata.name}");

            switch (currentPod.status.phase)
            {
                case nameof(ItemStatus.Running):
                    return ItemStatus.Running;
                case nameof(ItemStatus.Pending):
                    return ItemStatus.Pending;
                case nameof(ItemStatus.Succeeded):
                    return ItemStatus.Succeeded;
                case nameof(ItemStatus.Failed):
                    return ItemStatus.Failed;
                case nameof(ItemStatus.Unknown):
                    return ItemStatus.Unknown;
                default:
                    return ItemStatus.Unknown;
            }
        }

        public enum ItemStatus
        {
            Pending,
            Running,
            Succeeded,
            Failed,
            Unknown,
            NotCreated
        }
    }
}
