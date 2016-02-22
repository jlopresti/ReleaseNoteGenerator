namespace ReleaseNoteGenerator.Console.Publlsher
{
    internal interface IPublisher
    {
        bool Publish(string output);
    }
}