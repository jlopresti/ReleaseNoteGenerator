using System;
using log4net;
using ReleaseNoteGenerator.Console.Exceptions;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.Publlsher
{
    class PublisherFactory : IPublisherFactory
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(GithubSourceControlFactory));
        public IPublisher GetProvider(Config settings)
        {
            Guard.IsNotNull(settings);

            _logger.Debug("[PBS] Try getting publisher provider from config");
            var provider = settings.Publish.GetTypeProvider<IPublisher>();
            if (provider != null)
            {
                _logger.DebugFormat("[PBS] Publisher provider found : {0}", provider.Name);
                return (IPublisher)Activator.CreateInstance(provider, settings.Publish);
            }
            throw new ProviderNotFoundException(settings.Publish.GetProvider());
        }
    }
}