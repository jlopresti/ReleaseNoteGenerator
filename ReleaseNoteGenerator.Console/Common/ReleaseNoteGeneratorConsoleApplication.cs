using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly ITemplateProviderFactory _templateProviderFactory;
        private readonly IPublisherFactory _publisherFactory;
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteGeneratorConsoleApplication));

        public ReleaseNoteGeneratorConsoleApplication(ISourceControlFactory sourceControlFactory, IIssueTrackerFactory issueTrackerFactory,
            ITemplateProviderFactory templateProviderFactory, IPublisherFactory publisherFactory)
        {
            _sourceControlProvider = sourceControlFactory;
            _issueTrackerFactory = issueTrackerFactory;
            _templateProviderFactory = templateProviderFactory;
            _publisherFactory = publisherFactory;
        }

        public async Task<int> Run(string[] args)
        {
            var settings = new Settings();
            if (Parser.Default.ParseArguments(args, settings))
            {
                _logger.Info("SUCCESS");
                var config = File.ReadAllText(settings.ConfigPath).ToObject<Config>();
                var sourceControl = _sourceControlProvider.GetProvider(config.SourceControl);
                var issueTracker = _issueTrackerFactory.GetProvider(config.IssueTracker);
                var templateProvider = _templateProviderFactory.GetProvider(config.Template);
                var publisher = _publisherFactory.GetProvider(config.Publish);
                var issues = await issueTracker.GetIssues(settings.ReleaseNumber);
                var commits = await sourceControl.GetCommits(settings.ReleaseNumber);
                ApplyKeyExtractionFromMessage(issueTracker, commits, settings.IssueIdPattern);
                issues = issues.Distinct(new ReleaseNoteKeyComparer()).Cast<Issue>().ToList();
                commits = commits.Distinct(new ReleaseNoteKeyComparer()).Cast<Commit>().ToList();
                var binder = new ReleaseNoteBinder(commits,issues);
                var releaseNoteModel = binder.Bind();
                var output = templateProvider.Build(releaseNoteModel);
                var result = publisher.Publish(output);
                return result ? Constants.SUCCESS_EXIT_CODE : Constants.FAIL_EXIT_CODE;
            }
            return Constants.FAIL_EXIT_CODE;
        }

        private void ApplyKeyExtractionFromMessage(IIssueTrackerProvider issueTracker, List<Commit> commits, string pattern)
        {
            for (int index = 0; index < commits.Count; index++)
            {
                var commit = commits[index];
                commit.ExtractKeyFromTitle(pattern);
                if (commit.HasExtractedKey)
                {
                    var issue = issueTracker.GetIssue(commit.Id);
                    if (issue != null && !issue.Type.Equals("defect", StringComparison.InvariantCultureIgnoreCase))
                    {
                        commit.Id = issue.Id;
                        commit.Title = issue.Title;
                    }
                    else if (issue != null && issue.Type.Equals("defect", StringComparison.InvariantCultureIgnoreCase))
                    {
                        commits.Remove(commit);
                    }
                }
            }
        }
    }
}