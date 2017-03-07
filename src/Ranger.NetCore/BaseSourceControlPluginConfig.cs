using Ranger.NetCore.Common;

namespace Ranger.NetCore
{
    public class BaseSourceControlPluginConfig : ISourceControlPluginConfiguration
    {
        public string Provider { get; set; }
        public string MessageCommitPattern { get; set; }
        public string ExcludeCommitPattern { get; set; }
    }
}