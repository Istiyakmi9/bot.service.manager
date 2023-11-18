namespace bot.service.manager.Model
{
    public class FolderDiscovery
    {
        public List<FolderDetail>? Folders { get; set; }
        public string? TargetDirectory { get; set; }
        public string? RootDirectory { get; set; }
        public string? FolderPath { get; set; }
        public string? FolderName { get; set; }
    }

    public class FileDetail
    {
        public string FullPath { get; set; }
        public string FileName { get; set; }
    }

    public class FolderDetail
    {
        public string FullPath { get; set; }
        public string FolderName { get; set; }
    }

    public class KubectlModel
    {
        public string Command { get; set; }
        public bool IsWindow { get; set; } = false;
        public bool IsMicroK8 { get; set; } = false;
    }
}
