using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public List<Issue> GetIssues(string release)
        {
            // use LINQ syntax to retrieve issues
            var issues = from i in _client.Issues
                        from j in i.FixVersions
                        where j.Name == release
                        orderby i.Created
                        select new Issue
                        {
                            Title = i.Summary,
                            Id = i.Key.Value
                        };
            return issues.ToList();
        }
    }
}