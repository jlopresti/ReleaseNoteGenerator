using System;
using log4net;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Exceptions;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    class JiraIssueTrackerFactory : IIssueTrackerFactory
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(JiraIssueTrackerFactory));
        public IIssueTrackerProvider GetProvider(Config settings)
        {
            Guard.IsValidConfig(() => settings);
            Guard.ProviderRequired(() => settings.IssueTracker);

            _logger.Debug("[IT] Try getting issue tracker provider from config");
            var provider = settings.IssueTracker.GetTypeProvider<IIssueTrackerProvider>();
            if (provider != null)
            {
                Guard.ValidateConfigParameter(provider, () => settings.IssueTracker);
                _logger.DebugFormat("[IT] Issue tracker provider found : {0}", provider.Name);
                var it = (IIssueTrackerProvider)Activator.CreateInstance(provider, settings.IssueTracker);
                return new DistinctIssueProvider(it);
            }
            throw new ApplicationException($"No provider found with id : {settings.IssueTracker.GetProvider()}");
        }
    }
}