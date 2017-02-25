namespace Ranger.NetCore.Publisher
{
    public interface IPublisher
    {
        bool Publish(string releaseNumber, string output);
    }
}