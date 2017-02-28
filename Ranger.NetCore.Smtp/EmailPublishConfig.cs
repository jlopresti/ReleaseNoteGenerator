using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Smtp
{
    public class EmailPublishConfig : IPluginConfiguration
    {
        public string Server { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Ssl { get; set; }
        public string Provider { get; set; }
    }
}