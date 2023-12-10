namespace bot.service.manager.Model
{
    public class FileDetail
    {
        public string FullPath { get; set; }
        public string FileName { get; set; }
        public bool Status { get; set; }
        public string FileContent { get; set; }
        public string FileType { set; get; }
        public string PVSize { set; get; }
        public PodRootModel podRootModel { get; set; }
        public bool IsFolder { get; set; }
    }
}
