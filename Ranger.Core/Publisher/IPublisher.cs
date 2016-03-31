namespace Ranger.Core.Publisher
{
    public interface IPublisher
    {
        bool Publish(string releaseNumber, string output);
    }
}