using bot.service.manager.Model;

namespace bot.service.manager.IService
{
    public interface IActionService
    {
        Task<GitHubContent> RunFileService(GitHubContent fileDetail);
        Task<FileDetail> ReRunFileService(FileDetail fileDetail);
        Task<FileDetail> StopFileService(FileDetail fileDetail);
        Task<FileDetail> CheckStatusService(KubectlModel kubectlModel);
    }
}
