
using System.ComponentModel.DataAnnotations;
using Ranger.NetCore.Common;

namespace Ranger.NetCore.Jira.Configs
{
    public class JiraConfiguration : IPluginConfiguration
    {
        [Required]
        public string Host { get; set; }
        [Required]
        public string Project { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Provider { get; set; }
    }
}