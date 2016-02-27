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
    public class SourceControlFactory : ISourceControlFactory
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(SourceControlFactory));
        private readonly IIssueTrackerFactory _issueTrackerFactory;

        public SourceControlFactory(IIssueTrackerFactory issueTrackerFactory)
        {
            _issueTrackerFactory = issueTrackerFactory;
        }

        public ISourceControlProvider GetProvider(Config settings)
        {
            Guard.IsValidConfig(() => settings);
            Guard.ProviderRequired(() => settings.SourceControl);

            _logger.Debug("[SC] Try getting source control provider from config");
            var provider = settings.SourceControl.GetTypeProvider<ISourceControlProvider>();
            if (provider != null)
            {
                Guard.ValidateConfigParameter(provider, () => settings.SourceControl);
                _logger.DebugFormat("[SC] Source control provider found : {0}", provider.Name);
                var sc = (ISourceControlProvider)Activator.CreateInstance(provider, settings.SourceControl);
                return new DistinctCommitSourceControl(new EnrichCommitWithIssueTrackeProvider(sc, _issueTrackerFactory.GetProvider(settings), settings.SourceControl));
            }
            throw new ApplicationException($"No provider found with id : {settings.SourceControl.GetProvider()}");
        }
    }
}