using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using log4net;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.Common
{
    internal class ReleaseNoteGeneratorConsoleApplication : IConsoleApplication
    {
        private readonly ISourceControlFactory _sourceControlProvider;
        private readonly IIssueTrackerFactory _issueTrackerFactory;
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteGeneratorConsoleApplication));

        public ReleaseNoteGeneratorConsoleApplication(ISourceControlFactory sourceControlFactory, IIssueTrackerFactory issueTrackerFactory)
        {
            _sourceControlProvider = sourceControlFactory;
            _issueTrackerFactory = issueTrackerFactory;
        }

        public async Task<int> Run(string[] args)
        {
            var settings = new Settings();
            if (Parser.Default.ParseArguments(args, settings))
            {
                _logger.Info("SUCCESS");
                var sourceControl = _sourceControlProvider.GetProvider(settings);
                var issueTracker = _issueTrackerFactory.GetProvider(settings);

                var issues = issueTracker.GetIssues(settings.RelNumber);
                var commits = await sourceControl.GetCommits(settings.RelNumber);
                ApplyKeyExtractionFromMessage(commits, settings.IssueCommitPattern);
                issues = issues.Distinct(new ReleaseNoteKeyComparer()).Cast<Issue>().ToList();
                commits = commits.Distinct(new ReleaseNoteKeyComparer()).Cast<Commit>().ToList();
                var binder = new ReleaseNoteBinder(commits,issues);
                var releaseNoteModel = binder.Bind();
            }
            return Constants.FAIL_EXIT_CODE;
        }

        private void ApplyKeyExtractionFromMessage(List<Commit> commits, string pattern)
        {
            foreach (var commit in commits)
            {
                commit.ExtractKeyFromTitle(pattern);
            }
        }
    }
}