
using bot.service.manager.Model;

namespace bot.service.manager.IService
{
    public interface IFolderDiscoveryService
    {
        Task<FolderDiscovery> GetFolderDetailService(string targetDirectory);
        Task<string> RunCommandService();
    }
}
