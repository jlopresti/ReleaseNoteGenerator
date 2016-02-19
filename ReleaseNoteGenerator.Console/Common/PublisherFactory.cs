using System;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    class PublisherFactory : IPublisherFactory
    {
        public IPublisher GetProvider(Config settings)
        {
            switch (settings.PublishType)
            {
                case Publish.Local:
                    return new LocalPublisher(settings.Publish);
                default:
                    return new NoOpPublisher();
            }                       
        }
    }
}