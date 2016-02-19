namespace ReleaseNoteGenerator.Console.Common
{
    internal class NoOpPublisher : IPublisher
    {
        public bool Publish(string output)
        {
            return true;
        }
    }
}