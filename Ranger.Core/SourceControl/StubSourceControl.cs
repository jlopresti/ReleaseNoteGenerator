using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Ranger.Core.Common;
using Ranger.Core.Models.SourceControl;

namespace Ranger.Core.SourceControl
{
    [Provider("stub", ConfigurationType = typeof(GithubConfig))]
    public class StubSourceControl : ISourceControl
    {
        private GithubConfig _config;

        public StubSourceControl(GithubConfig config)
        {
            _config = config;
        }
        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            var commits = new List<Commit>();
            foreach (var prj in _config.ProjectConfigs)
            {
                if (prj.Project == "toto")
                {
                    commits.Add(new Commit()
                    {
                        Authors = new List<string> {"Jilo1"},
                        Title = "CLOUD-8679 : Hello world 1"
                    });
                    commits.Add(new Commit() {Authors = new List<string> {"Jilo2"}, Title = "SGR-2 : Hello world 2"});
                    commits.Add(new Commit() {Authors = new List<string> {"Jilo3"}, Title = "SGR-2 : Hello world 2bis"});
                    commits.Add(new Commit() {Authors = new List<string> {"Jilo4"}, Title = "SGR-3 : Hello world 3"});
                    commits.Add(new Commit() {Authors = new List<string> {"Jilo5"}, Title = "Hello world"});
                }
                else if(prj.Project == "test")
                {
                    commits.Add(new Commit() { Authors = new List<string> { "Jilo2" }, Title = "SGR-54642 : jsd lfj sldjkfl jHello world 2" });
                }
            }
            //await Task.Delay(5000);
            return commits;
        }
    }
}
