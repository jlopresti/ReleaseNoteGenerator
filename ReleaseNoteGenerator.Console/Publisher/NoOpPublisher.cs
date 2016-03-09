using Ranger.Console.Common;

namespace Ranger.Console.Publisher
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