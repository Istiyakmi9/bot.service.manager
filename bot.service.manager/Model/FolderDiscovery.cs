namespace bot.service.manager.Model
{
    public class FolderDiscovery
    {
        public List<string>? Folders { get; set; }
        public List<FileDetail>? Files { get; set; }
        public string? TargetDirectory { get; set; }
        public string? RootDirectory { get; set; }
    }

    public class FileDetail
    {
        public string FullPath { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
    }
}
