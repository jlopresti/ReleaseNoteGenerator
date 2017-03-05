using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json.Linq;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.SourceControl
{
    public class EnrichCommitWithIssueTracker : ICommitEnrichment
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(EnrichCommitWithIssueTracker));
        private readonly IIssueTracker _issueTracker;
        public BaseSourceControlPluginConfig Config { get; set; }

        public EnrichCommitWithIssueTracker(IIssueTracker issueTracker, IReleaseNoteConfiguration configurationManager)
        {
            _issueTracker = issueTracker;
            Config = configurationManager.GetSourceControlConfig<BaseSourceControlPluginConfig>();
        }


        public async Task<List<CommitInfo>> EnrichCommitWithData(List<CommitInfo> result)
        {
            if (!string.IsNullOrEmpty(Config.ExcludeCommitPattern))
            {
                result = result.Where(x => !Regex.IsMatch(x.Title, Config.ExcludeCommitPattern, RegexOptions.IgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(Config.MessageCommitPattern))
            {
                await ApplyKeyExtractionFromMessage(result, Config.MessageCommitPattern);
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

    }
}