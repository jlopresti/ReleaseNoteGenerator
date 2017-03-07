using System.Collections.Generic;
using Ranger.NetCore.Common;

namespace Ranger.NetCore.Github.Configs
{
    public class GithubConfig : GithubProjectConfig, ISourceControlPluginConfiguration
    {
        private IEnumerable<GithubProjectConfig> _projectConfigs;

        public IEnumerable<GithubProjectConfig> ProjectConfigs
        {
            get { return _projectConfigs; }
            set
            {
                _projectConfigs = value;
                foreach (var projectConfig in value)
                {
                    projectConfig.Setup(this);
                }
            }
        }

        public string Provider { get; set; }
        public string ExcludeCommitPattern { get; set; }
    }
}