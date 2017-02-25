using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.SourceControl
{
    [Provider("stub", ConfigurationType = typeof(GithubConfig))]
    public class StubSourceControl : ISourceControl
    {
        private GithubConfig _config;

        public StubSourceControl(/*GithubConfig config*/)
        {
            //_config = config;
        }
        public async Task<List<CommitInfo>> GetCommits(string releaseNumber)
        {
            var commits = new List<CommitInfo>();
            foreach (var prj in _config.ProjectConfigs)
            {
                if (prj.Project == "toto")
                {
                    commits.Add(new CommitInfo()
                    {
                        Authors = new List<string> {"Jilo1"},
                        Title = "CLOUD-8679 : Hello world 1"
                    });
                    commits.Add(new CommitInfo() {Authors = new List<string> {"Jilo2"}, Title = "SGR-2 : Hello world 2"});
                    commits.Add(new CommitInfo() {Authors = new List<string> {"Jilo3"}, Title = "SGR-2 : Hello world 2bis"});
                    commits.Add(new CommitInfo() {Authors = new List<string> {"Jilo4"}, Title = "SGR-3 : Hello world 3"});
                    commits.Add(new CommitInfo() {Authors = new List<string> {"Jilo5"}, Title = "Hello world"});
                }
                else if(prj.Project == "test")
                {
                    commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo2" }, Title = "SGR-54642 : jsd lfj sldjkfl jHello world 2" });
                }
            }
            //await Task.Delay(5000);
            return commits;
        }

        public async Task<List<CommitInfo>> GetCommitsFromPastRelease(string release)
        {
            var commits = new List<CommitInfo>();
            foreach (var prj in _config.ProjectConfigs)
            {
                if (prj.Project == "toto")
                {
                    commits.Add(new CommitInfo()
                    {
                        Authors = new List<string> { "Jilo1" },
                        Title = "CLOUD-8679 : Hello world 1"
                    });
                    commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo2" }, Title = "SGR-2 : Hello world 2" });
                    commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo3" }, Title = "SGR-2 : Hello world 2bis" });
                    commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo4" }, Title = "SGR-3 : Hello world 3" });
                    commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo5" }, Title = "Hello world" });
                }
                else if (prj.Project == "test")
                {
                    commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo2" }, Title = "SGR-54642 : jsd lfj sldjkfl jHello world 2" });
                }
            }
            //await Task.Delay(5000);
            return commits;
        }
    }
}
