using Ranger.NetCore.Common;

namespace Ranger.NetCore.Publisher
{
    [Provider("noop")]
    public class NoOpPublisher : IPublisher
    {
        public bool Publish(string releaseNumber, string output)
        {
            return true;
        }
    }
}