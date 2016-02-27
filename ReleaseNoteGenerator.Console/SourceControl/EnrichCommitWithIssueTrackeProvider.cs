using System;
using System.Collections.Generic;
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

        public EnrichCommitWithIssueTrackeProvider(ISourceControlProvider innerSourceControlProvider, 
            IIssueTrackerProvider issueTrackerProvider, 
            JObject config)
        {
            Guard.IsNotNull(() => innerSourceControlProvider, () => issueTrackerProvider,() => config);

            _innerSourceControlProvider = innerSourceControlProvider;
            _issueTrackerProvider = issueTrackerProvider;
            _pattern = config.GetCommitMessagePattern();
        }

        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            Guard.IsNotNullOrEmpty(() => releaseNumber);

            var result = await _innerSourceControlProvider.GetCommits(releaseNumber);
            if (!string.IsNullOrWhiteSpace(_pattern))
            {
                ApplyKeyExtractionFromMessage(result, _pattern);
            }
            return result;
        }

        private void ApplyKeyExtractionFromMessage(List<Commit> commits, string pattern)
        {
            _logger.Debug("[SC] Try extracting issue tracker key from commit message");
            for (int index = 0; index < commits.Count; index++)
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
                }
            }
        }
    }
}