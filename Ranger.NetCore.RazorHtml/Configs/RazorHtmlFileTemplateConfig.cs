using System.ComponentModel.DataAnnotations;
using System.IO;
using Ranger.NetCore.Common;

namespace Ranger.NetCore.RazorHtml.Configs
{
    public class RazorHtmlFileTemplateConfig : IPluginConfiguration
    {
        [Required, FileExists]
        public string File { get; set; }
        public string Provider { get; set; }
    }
}