namespace ReleaseNoteGenerator.Console.Common
{
    internal interface IPublisher
    {
        bool Publish(string output);
    }
}