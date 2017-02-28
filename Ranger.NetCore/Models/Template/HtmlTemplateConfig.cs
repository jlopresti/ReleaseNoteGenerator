using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Models.Template
{
    public class HtmlTemplateConfig : IPluginConfiguration
    {
        public string Html { get; set; }
        public string Provider { get; set; }
    }
}