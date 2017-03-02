using System.ComponentModel.DataAnnotations;

namespace Ranger.NetCore.RazorHtml.Configs
{
    public class RazorHtmlTemplateConfig : IPluginConfiguration
    {
        [Required]
        public string Html { get; set; }
        public string Provider { get; set; }
    }
}