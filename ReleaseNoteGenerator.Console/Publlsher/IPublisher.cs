namespace ReleaseNoteGenerator.Console.Publlsher
{
    internal interface IPublisher
    {
        bool Publish(string releaseNumber, string output);
    }
}