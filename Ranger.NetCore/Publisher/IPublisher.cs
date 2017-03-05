namespace Ranger.NetCore.Publisher
{
    public interface IPublisher : IRangerPlugin
    {
        bool Publish(string releaseNumber, string output);
    }
}