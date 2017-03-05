using Ranger.NetCore.Common;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Plugins
{
    public class NoOpPublisher : IPublisher
    {
        public string PluginId => "noop";

        public bool Publish(string releaseNumber, string output)
        {
            return true;
        }

        public void ActivatePlugin()
        {
            
        }
    }
}