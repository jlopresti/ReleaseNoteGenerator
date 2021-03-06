using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using Ranger.Core.Helpers;
using Ranger.Core.IssueTracker;
using Ranger.Core.Models;
using Ranger.Core.Models.SourceControl;

namespace Ranger.Core.SourceControl
{
    public class EnrichCommitWithIssueTracker : ISourceControl
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(EnrichCommitWithIssueTracker));
        private readonly ISourceControl _innerSourceControl;
        private readonly IIssueTracker _issueTracker;
        private string _pattern;
        private string _excludePattern;

        public EnrichCommitWithIssueTracker(ISourceControl innerSourceControl, 
            IIssueTracker issueTracker, 
            IReleaseNoteConfiguration config)
        {
            Guard.IsNotNull(() => innerSourceControl, () => issueTracker,() => config);

            _innerSourceControl = innerSourceControl;
            _issueTracker = issueTracker;
            _pattern = config.Config.SourceControl.GetCommitMessagePattern();
            _excludePattern = config.Config.SourceControl.GetExcludeCommitPattern();
        }

        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            Guard.IsNotNullOrEmpty(() => releaseNumber);

            var result = await _innerSourceControl.GetCommits(releaseNumber);
            result = EnrichCommitWithData(result);
            return result;
        }

        private List<Commit> EnrichCommitWithData(List<Commit> result)
        {
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
                    var issue = _issueTracker.GetIssue(commit.Id);
                    if (issue != null && !issue.Type.Equals("defect", StringComparison.InvariantCultureIgnoreCase))
                    {
                        commit.Id = issue.Id;
                        commit.Title = issue.Title;
                        commit.AdditionalData = issue.AdditionalData;
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

        public async Task<List<Commit>> GetCommitsFromPastRelease(string release)
        {
            Guard.IsNotNullOrEmpty(() => release);

            var result = await _innerSourceControl.GetCommits(release);
            result = EnrichCommitWithData(result);
            return result;
        }
    }
}