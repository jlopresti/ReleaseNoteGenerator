using System.Collections.Generic;
using Ranger.NetCore.Models.Binder;
using Ranger.NetCore.Models.IssueTracker;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.Linker
{
    public interface IReleaseNoteLinker
    {
        List<ReleaseNoteEntry> Link(List<CommitInfo> commits, List<Issue> issues);
    }
}