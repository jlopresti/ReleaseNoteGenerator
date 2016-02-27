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
        readonly ILog _logger = LogManager.GetLogger(typeof(SourceControlFactory));
        public ITemplateProvider GetProvider(Config settings)
        {
            Guard.IsValidConfig(() => settings);
            Guard.ProviderRequired(() => settings.Template);

            _logger.Debug("[TMP] Try getting template provider from config");
            var provider = settings.Template.GetTypeProvider<ITemplateProvider>();
            if (provider != null)
            {
                Guard.ValidateConfigParameter(provider, () => settings.Template);
                _logger.DebugFormat("[TMP] Template provider found : {0}", provider.Name);
                return (ITemplateProvider)Activator.CreateInstance(provider, settings.Template);
            }
            throw new ApplicationException($"No provider found with id : {settings.Template.GetProvider()}");
        }
    }
}