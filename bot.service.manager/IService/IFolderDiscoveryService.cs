using Core.Pipeline.Model;

namespace Core.Pipeline.IService
{
    public interface IFolderDiscoveryService
    {
        Task<FolderDiscovery> GetFolderDetailService(string targetDirectory);
        Task<string> RunCommandService();
    }
}
