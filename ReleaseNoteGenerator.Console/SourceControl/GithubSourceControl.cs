using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Octokit;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models;
using Commit = ReleaseNoteGenerator.Console.Models.Commit;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class GithubSourceControl : ISourceControlProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(GithubSourceControl));
        private readonly GithubConfig _config;
        private GitHubClient _client;

        public GithubSourceControl(string configPath)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                _logger.Error("Github config must be provided", new NullReferenceException("configPath"));
                return;
            }
            if (!File.Exists(configPath))
            {
                _logger.Error("Github config not found, please check config path", new FileNotFoundException($"{configPath} doesn't exist."));
                return;
            }
            _config = File.ReadAllText(configPath).ToObject<GithubConfig>();
            if (_config == null)
            {
                _logger.Error("Invalid github config", new JsonException("Json is invalid"));
                return;
            }

            _client = new GitHubClient(new Connection(new ProductHeaderValue("ReleaseNote"), new Uri(new Uri(_config.Host, UriKind.Absolute), new Uri("/api/v3/", UriKind.Relative))))
                { Credentials = new Octokit.Credentials(_config.Login, _config.Apikey) };
        }

        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            var compare = await _client.Repository.Commit.Compare(_config.Owner, _config.Project, "master", string.Format(_config.ReleaseBranchPattern, releaseNumber));
            return compare.Commits.Where(x => !x.Commit.Message.StartsWith("Merge", StringComparison.InvariantCultureIgnoreCase))
                .Select(x => new Commit { Title = x.Commit.Message, Author = x.Author!= null ? x.Author.Login : "Unknown", Url = x.HtmlUrl}).ToList();
        }
    }
}