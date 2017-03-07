using System.ComponentModel.DataAnnotations;
using Ranger.NetCore.Common;

namespace Ranger.NetCore.RazorHtml.Configs
{
    public class RazorHtmlTemplateConfig : IPluginConfiguration
    {
        [Required]
        public string Html { get; set; }
        public string Provider { get; set; }
    }
}