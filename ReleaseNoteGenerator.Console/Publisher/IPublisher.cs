namespace ReleaseNoteGenerator.Console.Publisher
{
    public interface IPublisher
    {
        bool Publish(string releaseNumber, string output);
    }
}