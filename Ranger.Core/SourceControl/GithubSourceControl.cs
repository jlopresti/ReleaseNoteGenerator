using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using Ranger.Core.Common;
using Ranger.Core.Models.SourceControl;
using Commit = Ranger.Core.Models.SourceControl.Commit;

namespace Ranger.Core.SourceControl
{
    [Provider("github", ConfigurationType = typeof(GithubConfig))]
    [ConfigurationParameterValidation("projectConfigs")]
    public class GithubSourceControl : ISourceControl
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(GithubSourceControl));
        private readonly GithubConfig _config;

        public GithubSourceControl(GithubConfig config)
        {
            _config = config;
            if (_config == null)
            {
                _logger.Error("Invalid github config", new JsonException("Json is invalid"));
                return;
            }
        }

        private GitHubClient CreateGithubClient(GithubProjectConfig config)
        {
            return new GitHubClient(new Connection(new ProductHeaderValue("ReleaseNote"), new Uri(new Uri(config.Host, UriKind.Absolute), new Uri("/api/v3/", UriKind.Relative))))
            { Credentials = new Octokit.Credentials(config.Login, config.Apikey) };
        }

        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            var commits = new List<Commit>();
            foreach (var projectConfig in _config.ProjectConfigs)
            {
                var client = CreateGithubClient(projectConfig);
                try
                {
                    var branchRef = await client.Repository.GetBranch(_config.Owner, projectConfig.Project,
                        string.Format(_config.ReleaseBranchPattern, releaseNumber));
                    if (branchRef != null)
                    {
                        var compare =
                            await
                                client.Repository.Commit.Compare(_config.Owner, projectConfig.Project,
                                    _config.ProdBranch,
                                    string.Format(_config.ReleaseBranchPattern, releaseNumber));
                        var result = compare.Commits.Select(x =>
                        {
                            var c = new Commit
                            {
                                Title = x.Commit.Message,
                                Url = x.HtmlUrl,
                                Project = projectConfig.Project
                            };
                            c.Authors.Add(x.Author?.Login);
                            return c;
                        }).ToList();
                        commits.AddRange(result);
                    }
                }
                catch (NotFoundException ex)
                {
                }
            }
            return commits;
        }
    }
}