using Ranger.Core.Common;

namespace Ranger.Core.Publisher
{
    [Provider("noop")]
    internal class NoOpPublisher : IPublisher
    {
        public bool Publish(string releaseNumber, string output)
        {
            return true;
        }
    }
}