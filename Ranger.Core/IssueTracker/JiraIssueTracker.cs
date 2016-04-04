using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atlassian.Jira;
using Newtonsoft.Json.Linq;
using Ranger.Core.Common;
using Ranger.Core.Helpers;
using Ranger.Core.Models.IssueTracker;
using Issue = Ranger.Core.Models.IssueTracker.Issue;

namespace Ranger.Core.IssueTracker
{
    [Provider("jira", ConfigurationType = typeof(JiraConfig))]
    [ConfigurationParameterValidation("host", "login", "password", "project")]
    internal class JiraIssueTracker : IIssueTracker
    {
        private readonly JiraConfig _config;
        private readonly Jira _client;

        public JiraIssueTracker(JiraConfig config)
        {
            Guard.IsNotNull(() => config);

            _config = config;

            Guard.IsNotNull(() => _config);

            _client = Jira.CreateRestClient(_config.Host, _config.Login, _config.Password);
        }

        public async Task<List<Issue>> GetIssues(string release)
        {
            Guard.IsNotNullOrEmpty(() => release);

            var issues = await _client.GetIssuesFromJqlAsync($"project = {_config.Project} AND fixVersion = {release}", null, 0, new CancellationToken());
            var result = issues.Select(x =>
            {
                var issue = new Issue {Id = x.Key.Value, Title = x.Summary, Type = x.Type.Name};
                issue.AdditionalData.Add("Components", x.Components.Select(_=>_.Name).ToList());
                return issue;
            }).ToList();
            return result;
        }

        public Issue GetIssue(string id)
        {
            Guard.IsNotNullOrEmpty(() => id);

            var issue = _client.GetIssue(id);
            var result = new Issue { Id = issue.Key.Value, Title = issue.Summary, Type = issue.Type.Name };
            result.AdditionalData.Add("Components", issue.Components.Select(_ => _.Name).ToList());
            return result;
        }
    }
}