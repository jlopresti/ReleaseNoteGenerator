using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    class PublisherFactory : IPublisherFactory
    {
        public IPublisher GetProvider(JObject settings)
        {
            if (string.IsNullOrEmpty(settings.ConfigPath))
                return new NoOpPublisher();
            return new LocalPublisher(settings.ConfigPath);
        }
    }
}