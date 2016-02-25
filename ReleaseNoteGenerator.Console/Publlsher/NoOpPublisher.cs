using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.Publlsher
{
    [Provider("noop")]
    internal class NoOpPublisher : IPublisher
    {
        public bool Publish(string output)
        {
            return true;
        }
    }
}