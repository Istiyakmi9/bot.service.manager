using bot.service.manager.Model;

namespace bot.service.manager.IService
{
    public interface IActionService
    {
        Task<string> RunFileService(FileDetail fileDetail);
        Task<string> ReRunFileService(FileDetail fileDetail);
        Task<string> StopFileService(FileDetail fileDetail);
        Task<string> CheckStatusService(FileDetail fileDetail);
    }
}
