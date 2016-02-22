using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Publlsher
{
    internal interface IPublisherFactory
    {
        IPublisher GetProvider(Config settings);
    }
}