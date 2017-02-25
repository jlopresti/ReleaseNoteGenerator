using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.SourceControl
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

        public async Task<List<CommitInfo>> GetCommits(string releaseNumber)
        {
            Guard.IsNotNullOrEmpty(() => releaseNumber);

            var result = await _innerSourceControl.GetCommits(releaseNumber);
            result = await EnrichCommitWithData(result);
            return result;
        }

        private async Task<List<CommitInfo>> EnrichCommitWithData(List<CommitInfo> result)
        {
            if (!string.IsNullOrEmpty(_excludePattern))
            {
                result = result.Where(x => !Regex.IsMatch(x.Title, _excludePattern, RegexOptions.IgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(_pattern))
            {
                await ApplyKeyExtractionFromMessage(result, _pattern);
            }
            return result;
        }

        private async Task ApplyKeyExtractionFromMessage(List<CommitInfo> commits, string pattern)
        {
            _logger.Debug("[SC] Try extracting issue tracker key from commit message");
            for (int index = commits.Count-1; index >= 0; index--)
            {
                var commit = commits[index];
                commit.ExtractKeyFromTitle(pattern);
                if (commit.HasExtractedKey)
                {
                    var issue = await _issueTracker.GetIssue(commit.Id);
                    if (issue != null && !issue.Type.Equals("defect", StringComparison.CurrentCultureIgnoreCase))
                    {
                        commit.Id = issue.Id;
                        commit.Title = issue.Title;
                        commit.AdditionalData = issue.AdditionalData;
                    }
                    else if (issue != null && issue.Type.Equals("defect", StringComparison.CurrentCultureIgnoreCase))
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

        public async Task<List<CommitInfo>> GetCommitsFromPastRelease(string release)
        {
            Guard.IsNotNullOrEmpty(() => release);

            var result = await _innerSourceControl.GetCommits(release);
            result = await EnrichCommitWithData(result);
            return result;
        }
    }
}