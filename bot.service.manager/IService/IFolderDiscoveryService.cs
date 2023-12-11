
using bot.service.manager.Model;

namespace bot.service.manager.IService
{
    public interface IFolderDiscoveryService
    {
        Task<List<GitHubContent>> GetFolderDetailService(string targetDirectory);
        Task<string> RunCommandService(KubectlModel kubectlModel);
        Task<List<GitHubContent>> GetAllFileService(string targetDirectory);
    }
}
