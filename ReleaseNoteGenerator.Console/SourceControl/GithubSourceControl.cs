using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using ReleaseNoteGenerator.Console.Common;
using SC = ReleaseNoteGenerator.Console.Models.SourceControl;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    [Provider("github")]
    [ConfigurationParameterValidation("host", "login", "apikey","owner","project", "prodBranch", "releaseBranchPattern")]
    public class GithubSourceControl : ISourceControlProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(GithubSourceControl));
        private readonly SC.GithubConfig _config;
        private GitHubClient _client;

        public GithubSourceControl(JObject configPath)
        {
            _config = configPath.ToObject<SC.GithubConfig>();
            if (_config == null)
            {
                _logger.Error("Invalid github config", new JsonException("Json is invalid"));
                return;
            }

            _client = new GitHubClient(new Connection(new ProductHeaderValue("ReleaseNote"), new Uri(new Uri(_config.Host, UriKind.Absolute), new Uri("/api/v3/", UriKind.Relative))))
                { Credentials = new Octokit.Credentials(_config.Login, _config.Apikey) };
        }

        public async Task<List<SC.Commit>> GetCommits(string releaseNumber)
        {
            var compare = await _client.Repository.Commit.Compare(_config.Owner, _config.Project, "master", string.Format(_config.ReleaseBranchPattern, releaseNumber));
            return compare.Commits.Where(x => !x.Commit.Message.StartsWith("Merge", StringComparison.InvariantCultureIgnoreCase))
                .Select(x => new SC.Commit { Title = x.Commit.Message, Author = x.Author!= null ? x.Author.Login : "Unknown", Url = x.HtmlUrl}).ToList();
        }
    }
}