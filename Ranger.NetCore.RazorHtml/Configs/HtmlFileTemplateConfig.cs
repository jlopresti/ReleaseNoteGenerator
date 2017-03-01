using System.ComponentModel.DataAnnotations;

namespace Ranger.NetCore.RazorHtml.Configs
{
    public class HtmlFileTemplateConfig : IPluginConfiguration
    {
        [Required]
        public string File { get; set; }
        public string Provider { get; set; }
    }
}