using Ranger.NetCore.Common;

namespace Ranger.NetCore.Publisher
{
    public interface IPublisherPlugin : IRangerPlugin
    {
        bool Publish(string releaseNumber, string output);
    }
}