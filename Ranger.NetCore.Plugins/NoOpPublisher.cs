using Ranger.NetCore.Common;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Plugins
{
    [Provider("noop")]
    public class NoOpPublisher : IPublisher
    {
        public bool Publish(string releaseNumber, string output)
        {
            return true;
        }

        public void ActivatePlugin()
        {
            
        }
    }
}