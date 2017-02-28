using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Models.Publisher
{
    internal class LocalPublishConfig : IPluginConfiguration
    {
        public string OutputFile { get; set; }
        public string Provider { get; set; }
    }
}