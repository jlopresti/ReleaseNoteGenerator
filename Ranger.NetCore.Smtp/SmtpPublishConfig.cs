using System.ComponentModel.DataAnnotations;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Smtp
{
    public class SmtpPublishConfig : IPluginConfiguration
    {
        [Required]
        public string Server { get; set; }
        [EmailAddress]
        public string From { get; set; }
        [EmailAddress]
        public string To { get; set; }
        [Range(1, 65535)]
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Ssl { get; set; }
        public string Provider { get; set; }
    }
}