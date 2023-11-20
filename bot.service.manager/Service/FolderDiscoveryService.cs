using bot.service.manager.IService;
using bot.service.manager.Model;
using static bot.service.manager.Service.PodHelper;

namespace bot.service.manager.Service
{
    public class FolderDiscoveryService : IFolderDiscoveryService
    {
        private readonly CommonService _commonService;
        private readonly PodHelper _podHelper;

        public FolderDiscoveryService(CommonService commonService, PodHelper podHelper)
        {
            _commonService = commonService;
            _podHelper = podHelper;
        }

        public async Task<FolderDiscovery> GetFolderDetailService(string targetDirectory)
        {
            if (string.IsNullOrEmpty(targetDirectory))
                targetDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "k8-workspace"));

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            var result = await GetAllDirectory(targetDirectory);
            result.RootDirectory = Directory.GetCurrentDirectory();
            return result;
        }

        public async Task<List<FileDetail>> GetAllFileService(string targetDirectory)
        {
            if (string.IsNullOrEmpty(targetDirectory))
                throw new Exception("Directory is invalid");

            if (!Directory.Exists(targetDirectory))
                throw new Exception("Directory not found");

            var result = await GetFilesAndFolder(targetDirectory);
            return result;
        }

        private async Task<List<FileDetail>> GetFilesAndFolder(string targetDirectory)
        {
            List<FileDetail> result = await GetAllFilesInDirectory(targetDirectory);

            // Find folders

            if (result == null)
            {
                result = new List<FileDetail> { new FileDetail() };
            }

            string[] subdirectories = Directory.GetDirectories(targetDirectory);
            if (subdirectories != null && subdirectories.Length > 0)
            {
                foreach (var folder in subdirectories)
                {
                    string folderName = "";
                    if (folder.Contains(@"\"))
                        folderName = folder.Split(@"\").Last();
                    else
                        folderName = folder.Split(@"/").Last();

                    result.Add(new FileDetail
                    {
                        FullPath = folder,
                        FileName = folderName,
                        IsFolder = true
                    });
                }
            }

            return result;
        }

        private async Task<List<FileDetail>> GetAllFilesInDirectory(string targetDirectory)
        {
            var files = new List<FileDetail>();
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            string serviceName = string.Empty;
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

                    var type = GetFileType(fileName);
                    type = type.ToUpper();

                    switch (type)
                    {
                        case nameof(FileType.DEPLOY):
                            serviceName = fileName.Split(".").First().Replace("deploy", "pod");
                            files.Add(await GetPodDetail(serviceName, filePath, fileName, type));
                            break;
                        case nameof(FileType.SERVICE):
                            serviceName = fileName.Split(".").First();
                            files.Add(new FileDetail
                            {
                                FullPath = filePath,
                                FileName = fileName,
                                Status = !string.IsNullOrEmpty(await GetServiceName(serviceName)) ? true : false,
                                FileType = type,
                                IsFolder = false
                            });
                            break;
                        case nameof(FileType.PV):
                            serviceName = fileName.Split(".").First();
                            files.Add(new FileDetail
                            {
                                FullPath = filePath,
                                FileName = fileName,
                                Status = !string.IsNullOrEmpty(await GetPersistanceVolumeStatus(serviceName)) ? true : false,
                                FileType = type,
                                PVSize = await GetPersistanceVolumeSize(serviceName),
                                IsFolder = false
                            });
                            break;
                        case nameof(FileType.PVC):
                            serviceName = fileName.Split(".").First();
                            files.Add(new FileDetail
                            {
                                FullPath = filePath,
                                FileName = fileName,
                                Status = !string.IsNullOrEmpty(await GetPersistanceVolumeClaimStatus(serviceName)) ? true : false,
                                FileType = type,
                                IsFolder = false
                            });
                            break;
                    }
                }
            }

            return await Task.FromResult(files);
        }

        private async Task<FileDetail> GetPodDetail(string serviceName, string filePath, string fileName, string type)
        {
            PodRootModel? podRootModel = await GetPodName(serviceName);
            ItemStatus status = ItemStatus.Unknown;

            if (podRootModel != null)
            {
                status = _podHelper.FindPodStatus(podRootModel!, serviceName.Replace(".yml", "").Replace(".yaml", ""));
            }

            return new FileDetail
            {
                FullPath = filePath,
                FileName = fileName,
                Status = ItemStatus.Running == status,
                FileType = type
            };
        }

        private string GetFileType(string fileName)
        {
            var file = fileName.Substring(0, fileName.IndexOf("."));
            var splittedFileNamePart = file.Split('-');
            int len = splittedFileNamePart.Length;

            return splittedFileNamePart[len - 1];
        }

        private async Task<FolderDiscovery> GetAllDirectory(string targetDirectory)
        {
            FolderDiscovery folderDiscovery = new FolderDiscovery();
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            folderDiscovery.FolderPath = targetDirectory;
            if (folderDiscovery.FolderPath.Contains(@"\"))
                folderDiscovery.FolderName = targetDirectory.Split(@"\").Last();
            else
                folderDiscovery.FolderName = targetDirectory.Split(@"/").Last();

            if (subdirectoryEntries != null && subdirectoryEntries.Length > 0)
            {
                folderDiscovery.Folders = new List<FolderDetail>();
                foreach (var folder in subdirectoryEntries)
                {
                    string folderName = "";
                    if (folder.Contains(@"\"))
                        folderName = folder.Split(@"\").Last();
                    else
                        folderName = folder.Split(@"/").Last();

                    folderDiscovery.Folders.Add(new FolderDetail
                    {
                        FullPath = folder,
                        FolderName = folderName
                    });
                }
            }
            return await Task.FromResult(folderDiscovery);
        }

        private async Task<string> GetServiceName(string serviceName)
        {
            string optional = " | awk '{print $1}'";
            KubectlModel kubectlModel = new KubectlModel
            {
                Command = $"get service {serviceName} {optional}",
                IsMicroK8 = true,
                IsWindow = false
            };
            var result = await _commonService.RunAllCommandService(kubectlModel);
            return result;
        }

        private async Task<string> GetPersistanceVolumeSize(string serviceName)
        {
            string optional = " | awk '{print $3}'";
            KubectlModel kubectlModel = new KubectlModel
            {
                Command = $"get pv {serviceName} {optional}",
                IsMicroK8 = true,
                IsWindow = false
            };
            var result = await _commonService.RunAllCommandService(kubectlModel);
            return result;
        }

        private async Task<string> GetPersistanceVolumeStatus(string serviceName)
        {
            string optional = " | awk '{print $1}'";
            KubectlModel kubectlModel = new KubectlModel
            {
                Command = $"get pv {serviceName} {optional}",
                IsMicroK8 = true,
                IsWindow = false
            };
            var result = await _commonService.RunAllCommandService(kubectlModel);
            return result;
        }

        private async Task<string> GetPersistanceVolumeClaimStatus(string serviceName)
        {
            string optional = "| awk '{print $1}'";
            KubectlModel kubectlModel = new KubectlModel
            {
                Command = $"get pvc {serviceName} {optional}",
                IsMicroK8 = true,
                IsWindow = false
            };
            var result = await _commonService.RunAllCommandService(kubectlModel);
            return result;
        }

        private async Task<PodRootModel?> GetPodName(string podName)
        {
            KubectlModel kubectlModel = new KubectlModel
            {
                Command = $"get pods -o json",
                IsMicroK8 = true,
                IsWindow = false
            };

            return await _commonService.RunCommandToJsonService(kubectlModel);
        }

        public async Task<string> RunCommandService(KubectlModel kubectlModel)
        {
            var result = await _commonService.RunAllCommandService(kubectlModel);
            return result;
        }
    }
}
