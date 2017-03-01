using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.SourceControl;
using Ranger.NetCore.SourceControl;

namespace Ranger.NetCore.Stubs.SourceControl
{
    [Provider("stub")]
    public class StubSourceControl : ISourceControl
    {
        private readonly IReleaseNoteConfiguration _config;

        public StubSourceControl(IReleaseNoteConfiguration config)
        {
            _config = config;
        }
        public async Task<List<CommitInfo>> GetCommits(string releaseNumber)
        {
            var commits = new List<CommitInfo>();

            commits.Add(new CommitInfo()
            {
                Authors = new List<string> { "Jilo1" },
                Title = "CLOUD-8679 : Hello world 1"
            });
            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo2" }, Title = "JRA-9 : Hello world 2" });
            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo3" }, Title = "SGR-2 : Hello world 2bis" });
            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo4" }, Title = "SGR-3 : Hello world 3" });
            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo5" }, Title = "Hello world" });

            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo2" }, Title = "SGR-54642 : jsd lfj sldjkfl jHello world 2" });


            return commits;
        }

        public async Task<List<CommitInfo>> GetCommitsFromPastRelease(string release)
        {
            var commits = new List<CommitInfo>();

            commits.Add(new CommitInfo()
            {
                Authors = new List<string> { "Jilo1" },
                Title = "CLOUD-8679 : Hello world 1"
            });
            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo2" }, Title = "SGR-2 : Hello world 2" });
            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo3" }, Title = "SGR-2 : Hello world 2bis" });
            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo4" }, Title = "SGR-3 : Hello world 3" });
            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo5" }, Title = "Hello world" });

            commits.Add(new CommitInfo() { Authors = new List<string> { "Jilo2" }, Title = "SGR-54642 : jsd lfj sldjkfl jHello world 2" });

            return commits;
        }

        public void ActivatePlugin()
        {
            
        }
    }
}
