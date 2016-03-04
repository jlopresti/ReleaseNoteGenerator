using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Models.SourceControl;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class EnrichCommitWithIssueTrackeProvider : ISourceControlProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(EnrichCommitWithIssueTrackeProvider));
        private readonly ISourceControlProvider _innerSourceControlProvider;
        private readonly IIssueTrackerProvider _issueTrackerProvider;
        private string _pattern;
        private string _excludePattern;

        public EnrichCommitWithIssueTrackeProvider(ISourceControlProvider innerSourceControlProvider, 
            IIssueTrackerProvider issueTrackerProvider, 
            JObject config)
        {
            Guard.IsNotNull(() => innerSourceControlProvider, () => issueTrackerProvider,() => config);

            _innerSourceControlProvider = innerSourceControlProvider;
            _issueTrackerProvider = issueTrackerProvider;
            _pattern = config.GetCommitMessagePattern();
            _excludePattern = config.GetExcludeCommitPattern();
        }

        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            Guard.IsNotNullOrEmpty(() => releaseNumber);

            var result = await _innerSourceControlProvider.GetCommits(releaseNumber);
            if (!string.IsNullOrEmpty(_excludePattern))
            {
                result = result.Where(x => !Regex.IsMatch(x.Title, _excludePattern, RegexOptions.IgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(_pattern))
            {
                ApplyKeyExtractionFromMessage(result, _pattern);
            }
            return result;
        }

        private void ApplyKeyExtractionFromMessage(List<Commit> commits, string pattern)
        {
            _logger.Debug("[SC] Try extracting issue tracker key from commit message");
            for (int index = commits.Count-1; index >= 0; index--)
            {
                var commit = commits[index];
                commit.ExtractKeyFromTitle(pattern);
                if (commit.HasExtractedKey)
                {
                    var issue = _issueTrackerProvider.GetIssue(commit.Id);
                    if (issue != null && !issue.Type.Equals("defect", StringComparison.InvariantCultureIgnoreCase))
                    {
                        commit.Id = issue.Id;
                        commit.Title = issue.Title;
                    }
                    else if (issue != null && issue.Type.Equals("defect", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _logger.Debug($"[SC] Removing commit with key : {issue.Id} from list, because it's a defect");
                        commits.Remove(commit);
                    }
                    else
                    {
                        _logger.Debug($"[SC] {commit.Id} not found");
                    }
                }
            }
        }
    }
}