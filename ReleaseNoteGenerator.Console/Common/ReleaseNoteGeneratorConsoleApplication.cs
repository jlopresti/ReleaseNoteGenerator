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

    public class ReleaseNoteBinder
    {
        private readonly List<Commit> _commits;
        private readonly List<Issue> _issues;

        public ReleaseNoteBinder(List<Commit> commits, List<Issue> issues)
        {
            _commits = commits;
            _issues = issues;
        }

        public List<ReleaseNoteEntry> Bind()
        {
            var onlyCommitedItems = GetOnlyCommitedItems();
            var onlyAttachedInIssuesTracker = GetOnlyInIssuesTracker();
            var commitedAndAttachedItems = GetCommitedAndAttachedItems();
            return new List<ReleaseNoteEntry>();
        }

        private List<ReleaseNoteEntry> GetCommitedAndAttachedItems()
        {
            var issueKeys = _commits.Select(x => x.Id).ToList();

            var entries = from i in _issues
                join c in _commits on i.Id equals c.Id
                select new ReleaseNoteEntry()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Author = c.Author,
                    CommitUrl = c.Url,
                    Status = Status.Ok
                };

            return entries.ToList();
        }

        private List<ReleaseNoteEntry> GetOnlyInIssuesTracker()
        {
            var issueKeys = _commits.Select(x => x.Id).ToList();

            return _commits.Where(x => !issueKeys.Contains(x.Id)).Select(x => new ReleaseNoteEntry
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                CommitUrl = x.Url,
                Status = Status.OnlyCommited
            }).ToList();
        }

        private List<ReleaseNoteEntry> GetOnlyCommitedItems()
        {
            var commitKeys = _issues.Select(x => x.Id).ToList();

            return _issues.Where(x => !commitKeys.Contains(x.Id)).Select(x => new ReleaseNoteEntry
            {
                Id = x.Id,
                Title = x.Title,
                Author = string.Empty,
                Status = Status.OnlyAttachedInIssueTracker
            }).ToList();
        }
    }
}