using System.ComponentModel.DataAnnotations;

namespace Ranger.NetCore.Plugins
{
    internal class LocalPublishConfig : IPluginConfiguration
    {
        [Required, D]
        public string OutputFile { get; set; }
        public string Provider { get; set; }
    }
}