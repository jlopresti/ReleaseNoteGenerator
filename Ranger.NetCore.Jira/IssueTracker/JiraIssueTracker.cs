using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Jira;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Models.IssueTracker;

namespace Ranger.NetCore.Jira.IssueTracker
{
    [Provider("jira", ConfigurationType = typeof(JiraConfiguration))]
    [ConfigurationParameterValidation("host", "login", "password", "project")]
    internal class JiraIssueTracker : IIssueTracker
    {
        private readonly JiraConfiguration _config;
        private readonly IJiraClient _client;

        public JiraIssueTracker(JiraConfiguration config)
        {
            //Guard.IsNotNull(() => config);

            //_config = config;

            //Guard.IsNotNull(() => _config);

            //_client = JiraClient.Create(new Uri(_config.Host));
            //_client.SetBasicAuthentication(_config.Login, _config.Password);
        }

        public async Task<List<Issue>> GetIssues(string release)
        {
            Guard.IsNotNullOrEmpty(() => release);

            var issues = await _client.Issue.SearchAsync($"project = {_config.Project} AND fixVersion = {release}", 500);
            var result = issues.Select(x =>
            {
                var issue = new Issue { Id = x.Key, Title = x.Fields.Summary, Type = x.Fields.IssueType.Name};
                issue.AdditionalData.Add("Components", x.Fields.Components.Select(_ => _.Name).ToList());
                return issue;
            }).ToList();
            return result;
        }

        public async Task<Issue> GetIssue(string id)
        {
            Guard.IsNotNullOrEmpty(() => id);

            var issue = await _client.Issue.GetAsync(id);
            var result = new Issue { Id = issue.Key, Title = issue.Fields.Summary, Type = issue.Fields.IssueType.Name };
            result.AdditionalData.Add("Components", issue.Fields.Components.Select(_ => _.Name).ToList());
            return result;
        }
    }
}