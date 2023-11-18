using bot.service.manager.IService;
using bot.service.manager.Model;

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

            foreach (string filePath in fileEntries)
            {
                string extension = Path.GetExtension(filePath);
                if (extension.Equals(".yml") || extension.Equals(".yaml"))
                {
                    string fileName = "";
                    if (filePath.Contains(@"\"))
                        fileName = filePath.Split(@"\").Last();
                    else
                        fileName = filePath.Split(@"/").Last();

                    folderDiscovery.Files.Add(new FileDetail
                    {
                        FullPath = filePath,
                        FileName = fileName
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
