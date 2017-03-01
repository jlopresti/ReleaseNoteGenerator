
namespace Ranger.NetCore.Jira.Configs
{
    public class JiraConfiguration : IPluginConfiguration
    {
        public string Host { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Project { get; set; }
        public string Provider { get; set; }
    }
}