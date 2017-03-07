using Ranger.NetCore.Common;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Plugins
{
    public class NoOpPublisherPlugin : IPublisherPlugin
    {
        public string PluginId => "noop";

        public bool Publish(string releaseNumber, string output)
        {
            return true;
        }

        public void Activate()
        {
            
        }
    }
}