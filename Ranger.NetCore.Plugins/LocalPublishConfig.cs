using System.ComponentModel.DataAnnotations;
using Ranger.NetCore.Common;

namespace Ranger.NetCore.Plugins
{
    public class LocalPublishConfig : IPluginConfiguration
    {
        [Required, DirectoryExists]
        public string OutputFile { get; set; }
        public string Provider { get; set; }
    }
}