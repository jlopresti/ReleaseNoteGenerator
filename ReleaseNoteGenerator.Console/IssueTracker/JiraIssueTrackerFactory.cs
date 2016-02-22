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
            _logger.Debug("Try getting issue tracker provider from config");
            var provider = settings.IssueTracker.GetTypeProvider<IIssueTrackerProvider>();
            if (provider != null)
            {
                _logger.DebugFormat("Issue tracker provider found : {0}", provider.Name);
                var it = (IIssueTrackerProvider)Activator.CreateInstance(provider, settings.IssueTracker);
                return new DistinctIssueProvider(it);
            }
            throw new ProviderNotFoundException(settings.IssueTracker.GetProvider());
        }
    }
}