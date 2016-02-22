using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atlassian.Jira;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using ReleaseNoteGenerator.Console.SourceControl;
using IT = ReleaseNoteGenerator.Console.Models.IssueTracker;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    [Provider("jira")]
    internal class JiraIssueTracker : IIssueTrackerProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(JiraIssueTracker));
        private readonly IT.JiraConfig _config;
        private Jira _client;

        public JiraIssueTracker(JObject configPath)
        {
            _config = configPath.ToObject<IT.JiraConfig>();
            if (_config == null)
            {
                _logger.Error("Invalid jira config", new JsonException("Json is invalid"));
                return;
            }

            _client = Jira.CreateRestClient(_config.Host, _config.Login, _config.Password);
        }

        public async Task<List<IT.Issue>> GetIssues(string release)
        {
            var issues = await _client.GetIssuesFromJqlAsync($"project = {_config.Project} AND fixVersion = {release}", null, 0, new CancellationToken());
            return issues.Select(x => new IT.Issue { Id = x.Key.Value, Title = x.Summary, Type = x.Type.Name }).ToList();
        }

        public IT.Issue GetIssue(string id)
        {
            var issue = _client.GetIssue(id);
            return new IT.Issue { Id = issue.Key.Value, Title = issue.Summary, Type = issue.Type.Name };
        }
    }
}