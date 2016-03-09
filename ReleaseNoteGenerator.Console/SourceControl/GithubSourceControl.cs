using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using Ranger.Console.Common;
using Ranger.Console.Models.SourceControl;
using Commit = Ranger.Console.Models.SourceControl.Commit;

namespace Ranger.Console.SourceControl
{
    [Provider("github")]
    [ConfigurationParameterValidation("host", "login", "apikey","owner","projects", "prodBranch", "releaseBranchPattern")]
    public class GithubSourceControl : ISourceControl
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(GithubSourceControl));
        private readonly GithubConfig _config;
        private GitHubClient _client;

        public GithubSourceControl(JObject configPath)
        {
            _config = configPath.ToObject<GithubConfig>();
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
            var commits = new List<Commit>();
            foreach (var project in _config.Projects)
            {
                var compare = await _client.Repository.Commit.Compare(_config.Owner, project, _config.ProdBranch, string.Format(_config.ReleaseBranchPattern, releaseNumber));
                var result = compare.Commits.Select(x =>
                {
                    var c = new Commit
                    {
                        Title = x.Commit.Message,
                        Url = x.HtmlUrl,
                        Project = project
                    };
                    c.Authors.Add(x.Author?.Login);
                    return c;
                }).ToList();
                commits.AddRange(result);
            }
            return commits;
        }
    }
}