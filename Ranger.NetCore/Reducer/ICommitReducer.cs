using System.Collections.Generic;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.Reducer
{
    public interface ICommitReducer
    {
        List<CommitInfo> MergeCommits(List<CommitInfo> result);
    }
}