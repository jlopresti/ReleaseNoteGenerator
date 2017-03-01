namespace Ranger.NetCore.Plugins
{
    internal class LocalPublishConfig : IPluginConfiguration
    {
        public string OutputFile { get; set; }
        public string Provider { get; set; }
    }
}