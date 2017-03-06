using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.Enrichment
{
    public interface ICommitEnrichment
    {
        void Setup();
        Task<List<CommitInfo>> EnrichCommitWithData(List<CommitInfo> result);
    }
}