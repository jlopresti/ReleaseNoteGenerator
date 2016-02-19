using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    class PublisherFactory : IPublisherFactory
    {
        public IPublisher GetProvider(Settings settings)
        {
            if (string.IsNullOrEmpty(settings.PublishConfig))
                return new NoOpPublisher();
            return new LocalPublisher(settings.PublishConfig);
        }
    }
}