using System.ComponentModel.DataAnnotations;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Models.Template
{
    public class HtmlFileTemplateConfig : IPluginConfiguration
    {
        [Required]
        public string File { get; set; }
        public string Provider { get; set; }
    }
}