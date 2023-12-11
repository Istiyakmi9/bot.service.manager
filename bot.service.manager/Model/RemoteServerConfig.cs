namespace bot.service.manager.Model
{
    public class RemoteServerConfig
    {
        public string host { set; get; }
        public string username { set; get; }
        public string password { set; get; }
        public string env { set; get; }
        public string workingDirectory { set; get; }
        public string owner { set; get; }
        public string repo { set; get; }
        public string accessToken { set; get; }
    }
}
