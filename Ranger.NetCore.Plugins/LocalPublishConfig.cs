using System.ComponentModel.DataAnnotations;

namespace Ranger.NetCore.Plugins
{
    internal class LocalPublishConfig : IPluginConfiguration
    {
        [Required, DirectoryExists]
        public string OutputFile { get; set; }
        public string Provider { get; set; }
    }
}