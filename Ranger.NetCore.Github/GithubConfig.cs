using System.Collections.Generic;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Github
{
    public class GithubConfig : GithubProjectConfig, IPluginConfiguration
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
    }
}