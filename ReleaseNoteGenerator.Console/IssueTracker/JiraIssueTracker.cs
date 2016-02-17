using System;
using System.IO;
using log4net;
using Newtonsoft.Json;
using Octokit;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    internal class JiraIssueTracker : IIssueTrackerProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(JiraIssueTracker));
        private readonly JiraConfig _config;

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

            _client = new GitHubClient(new Connection(new ProductHeaderValue("ReleaseNote"), new Uri(new Uri(_config.Host, UriKind.Absolute), new Uri("/api/v3/", UriKind.Relative))))
            { Credentials = new Octokit.Credentials(_config.Login, _config.Apikey) };
        }
    }
}