using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json.Linq;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.Enrichment
{
    public class EnrichCommitWithIssueTracker : ICommitEnrichment
    {
        private IIssueTrackerPlugin _issueTrackerPlugin;
        private readonly IProviderFactory _providerFactory;
        private readonly IReleaseNoteConfiguration _configurationManager;
        private ILog _logger;
        private BaseSourceControlPluginConfig _config;

        public EnrichCommitWithIssueTracker(IProviderFactory providerFactory, 
            IReleaseNoteConfiguration configurationManager, 
            ILog logger)
        {
            _providerFactory = providerFactory;
            _configurationManager = configurationManager;
            _logger = logger;
        }

        public void Setup()
        {
            _issueTrackerPlugin = _providerFactory.CreateIssueTracker(_configurationManager);
            _config = _configurationManager.GetSourceControlConfig<BaseSourceControlPluginConfig>();
        }


        public async Task<List<CommitInfo>> EnrichCommitWithData(List<CommitInfo> result)
        {
            if (!string.IsNullOrEmpty(_config.ExcludeCommitPattern))
            {
                result = result.Where(x => !Regex.IsMatch(x.Title, _config.ExcludeCommitPattern, RegexOptions.IgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(_config.MessageCommitPattern))
            {
                await ApplyKeyExtractionFromMessage(result, _config.MessageCommitPattern);
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
                    var issue = await _issueTrackerPlugin.GetIssue(commit.Id);
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