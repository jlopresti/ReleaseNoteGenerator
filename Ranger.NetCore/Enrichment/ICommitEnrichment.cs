using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.SourceControl
{
    public interface ICommitEnrichment
    {
        Task<List<CommitInfo>> EnrichCommitWithData(List<CommitInfo> result);
    }
}