using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Models.SourceControl;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class EnrichCommitWithIssueTrackeProvider : ISourceControlProvider
    {
        private readonly ISourceControlProvider _innerSourceControlProvider;
        private readonly IIssueTrackerProvider _issueTrackerProvider;
        private string _pattern;

        public EnrichCommitWithIssueTrackeProvider(ISourceControlProvider innerSourceControlProvider, 
            IIssueTrackerProvider issueTrackerProvider, 
            JObject config)
        {
            _innerSourceControlProvider = innerSourceControlProvider;
            _issueTrackerProvider = issueTrackerProvider;
            _pattern = config.GetCommitMessagePattern();
        }

        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            var result = await _innerSourceControlProvider.GetCommits(releaseNumber);
            if (!string.IsNullOrWhiteSpace(_pattern))
            {
                ApplyKeyExtractionFromMessage(result, _pattern);
            }
            return result;
        }

        private void ApplyKeyExtractionFromMessage(List<Commit> commits, string pattern)
        {
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
                        commits.Remove(commit);
                    }
                }
            }
        }
    }
}