using System;
using System.Reflection;
using log4net;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Exceptions;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class GithubSourceControlFactory : ISourceControlFactory
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(GithubSourceControlFactory));
        private readonly IIssueTrackerFactory _issueTrackerFactory;

        public GithubSourceControlFactory(IIssueTrackerFactory issueTrackerFactory)
        {
            _issueTrackerFactory = issueTrackerFactory;
        }

        public ISourceControlProvider GetProvider(Config settings)
        {
            _logger.Debug("Try getting source control provider from config");
            var provider = settings.SourceControl.GetTypeProvider<ISourceControlProvider>();
            if (provider != null)
            {
                _logger.DebugFormat("Source control provider found : {0}", provider.Name);
                var sc = (ISourceControlProvider)Activator.CreateInstance(provider, settings.SourceControl);
                return new DistinctCommitSourceControl(new EnrichCommitWithIssueTrackeProvider(sc, _issueTrackerFactory.GetProvider(settings), settings.SourceControl));
            }
            throw new ProviderNotFoundException(settings.SourceControl.GetProvider());
        }
    }
}