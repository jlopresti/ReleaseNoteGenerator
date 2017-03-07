using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.SourceControl
{
    public interface ISourceControlPlugin : IRangerPlugin
    {
        Task<List<CommitInfo>> GetCommits(string releaseNumber);
        Task<List<CommitInfo>> GetCommitsFromPastRelease(string release);
    }
}