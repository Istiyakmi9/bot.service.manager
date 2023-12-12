using bot.service.manager.Model;

namespace bot.service.manager.IService
{
    public interface IActionService
    {
        Task<GitHubContent> RunFileService(GitHubContent fileDetail);
        Task<GitHubContent> ReRunFileService(GitHubContent gitHubContent);
        Task<GitHubContent> StopFileService(GitHubContent gitHubContent);
        Task<GitHubContent> CheckStatusService(GitHubContent gitHubContent);
    }
}
