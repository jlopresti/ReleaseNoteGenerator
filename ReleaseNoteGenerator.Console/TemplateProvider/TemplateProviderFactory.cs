using System;
using log4net;
using ReleaseNoteGenerator.Console.Exceptions;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.TemplateProvider
{
    public class TemplateProviderFactory : ITemplateProviderFactory
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(GithubSourceControlFactory));
        public ITemplateProvider GetProvider(Config settings)
        {
            Guard.IsNotNull(settings);

            _logger.Debug("[TMP] Try getting template provider from config");
            var provider = settings.Template.GetTypeProvider<ITemplateProvider>();
            if (provider != null)
            {
                _logger.DebugFormat("[TMP] Template provider found : {0}", provider.Name);
                return (ITemplateProvider)Activator.CreateInstance(provider, settings.Template);
            }
            throw new ProviderNotFoundException(settings.Template.GetProvider());
        }
    }
}