using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atlassian.Jira;
using log4net;
using Newtonsoft.Json;
using Octokit;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.SourceControl;
using Issue = ReleaseNoteGenerator.Console.Models.Issue;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    internal class JiraIssueTracker : IIssueTrackerProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(JiraIssueTracker));
        private readonly JiraConfig _config;
        private Jira _client;

        public JiraIssueTracker(string configPath)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                _logger.Error("Jira config must be provided", new NullReferenceException("configPath"));
                return;
            }
            if (!File.Exists(configPath))
            {
                _logger.Error("Jira config not found, please check config path", new FileNotFoundException($"{configPath} doesn't exist."));
                return;
            }
            _config = File.ReadAllText(configPath).ToObject<JiraConfig>();
            if (_config == null)
            {
                _logger.Error("Invalid jira config", new JsonException("Json is invalid"));
                return;
            }

            _client = Jira.CreateRestClient(_config.Host, _config.Login, _config.Password);
        }

        public async Task<List<Issue>> GetIssues(string release)
        {
            var issues = await _client.GetIssuesFromJqlAsync($"project = {_config.Project} AND fixVersion = ${release}", null, 0, new CancellationToken());
            return issues.Select(x => new Issue { Id = x.Key.Value, Title = x.Summary, Type = x.Type.Name }).ToList();
        }

        public Issue GetIssue(string id)
        {
            var issue = _client.GetIssue(id);
            return new Issue { Id = issue.Key.Value, Title = issue.Summary, Type = issue.Type.Name };
        }
    }
}