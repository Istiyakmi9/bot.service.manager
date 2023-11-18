using bot.service.manager.IService;
using bot.service.manager.Model;
using System.Diagnostics;

namespace bot.service.manager.Service
{
    public class FolderDiscoveryService : IFolderDiscoveryService
    {
        public async Task<FolderDiscovery> GetFolderDetailService(string targetDirectory)
        {
            if (string.IsNullOrEmpty(targetDirectory))
                targetDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "k8-workspace"));

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            var result = GetAllFilesInDirectory(targetDirectory);
            result.RootDirectory = Directory.GetCurrentDirectory();
            return await Task.FromResult(result);
        }

        private FolderDiscovery GetAllFilesInDirectory(string targetDirectory)
        {
            FolderDiscovery folderDiscovery = new FolderDiscovery();
            folderDiscovery.Files = new List<FileDetail>();
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            foreach (string fileName in fileEntries)
            {
                string extension = Path.GetExtension(fileName);
                if (extension.Equals(".yml") || extension.Equals(".yaml"))
                {
                    folderDiscovery.Files.Add(new FileDetail
                    {
                        FullPath = fileName,
                        FileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1)
                    });
                }
            }
            folderDiscovery.FolderPath = targetDirectory;
            if (folderDiscovery.FolderPath.Contains(@"\"))
                folderDiscovery.FolderName = targetDirectory.Split(@"\").Last();
            else
                folderDiscovery.FolderName = targetDirectory.Split(@"/").Last();

            return folderDiscovery;
        }

        public async Task<string> RunCommandService(KubectlModel kubectlModel)
        {
            var result = await CommonService.RunAllCommandService(kubectlModel);
            return result;
        }
    }
}
