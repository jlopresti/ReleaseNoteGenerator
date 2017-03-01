using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Jira;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Jira.Configs;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.IssueTracker;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Jira.IssueTracker
{
    [Provider("jira")]
    internal class JiraIssueTracker : BaseIssueTrackerPlugin<JiraConfiguration>
    {
        private IJiraClient _client;

        public JiraIssueTracker(IReleaseNoteConfiguration configuration)
            : base(configuration)
        {

        }

        protected override void OnPluginActivated()
        {
            _client = JiraClient.Create(new Uri(Configuration.Host));
            if (!string.IsNullOrEmpty(Configuration.Login) && !string.IsNullOrEmpty(Configuration.Password))
            {
                _client.SetBasicAuthentication(Configuration.Login, Configuration.Password);
            }
        }

        public override async Task<List<Issue>> GetIssues(string release)
        {
            //Guard.IsNotNullOrEmpty(() => release);

            var issues = await _client.Issue.SearchAsync($"project = {Configuration.Project} AND fixVersion = {release}", 500);
            var result = issues.Select(x =>
            {
                var issue = new Issue { Id = x.Key, Title = x.Fields.Summary, Type = x.Fields.IssueType?.Name};
                issue.AdditionalData.Add("Components", x.Fields?.Components?.Select(_ => _.Name).ToList());
                return issue;
            }).ToList();
            return result;
        }

        public override async Task<Issue> GetIssue(string id)
        {
            //Guard.IsNotNullOrEmpty(() => id);

            var issue = await _client.Issue.GetAsync(id);
            var result = new Issue { Id = issue.Key, Title = issue.Fields.Summary, Type = issue.Fields.IssueType.Name };
            result.AdditionalData.Add("Components", issue.Fields.Components.Select(_ => _.Name).ToList());
            return result;
        }
    }
}