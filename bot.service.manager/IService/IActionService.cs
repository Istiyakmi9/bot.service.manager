using bot.service.manager.Model;

namespace bot.service.manager.IService
{
    public interface IActionService
    {
        Task<FileDetail> RunFileService(FileDetail fileDetail);
        Task<FileDetail> ReRunFileService(FileDetail fileDetail);
        Task<FileDetail> StopFileService(FileDetail fileDetail);
        Task<FileDetail> CheckStatusService(KubectlModel kubectlModel);
    }
}
