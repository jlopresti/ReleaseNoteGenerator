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
            _logger.Debug("Try getting template provider from config");
            var provider = settings.Template.GetTypeProvider<ITemplateProvider>();
            if (provider != null)
            {
                _logger.DebugFormat("Template provider found : {0}", provider.Name);
                return (ITemplateProvider)Activator.CreateInstance(provider, settings.Template);
            }
            throw new ProviderNotFoundException(settings.Template.GetProvider());
        }
    }
}