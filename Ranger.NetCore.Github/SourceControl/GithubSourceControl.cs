using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Octokit;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models.SourceControl;
using Ranger.NetCore.SourceControl;

namespace Ranger.NetCore.Github.SourceControl
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

        public async Task<List<CommitInfo>> GetCommits(string releaseNumber)
        {
            var commits = new List<CommitInfo>();
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
                            var c = new CommitInfo
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

        public async Task<List<CommitInfo>> GetCommitsFromPastRelease(string release)
        {
            var commits = new List<CommitInfo>();
            foreach (var projectConfig in _config.ProjectConfigs)
            {
                var client = CreateGithubClient(projectConfig);
                try
                {
                    var repoTags = await client.Repository.GetAllTags(_config.Owner, projectConfig.Project);
                    var tags = repoTags.Select(x => x.Name).ToList();
                    var releaseTagIndex = tags.IndexOf(release);
                    if (releaseTagIndex > 0)
                    {
                        var latestMasterBeforeRelase = tags[releaseTagIndex - 1];
                        var compare = await client.Repository.Commit.Compare(_config.Owner, projectConfig.Project,
                                    latestMasterBeforeRelase, release);
                        var result = compare.Commits.Select(x =>
                        {
                            var c = new CommitInfo
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