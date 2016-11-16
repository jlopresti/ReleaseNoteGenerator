using System.Collections.Generic;
using Ranger.Core.Models.Binder;
using Ranger.Core.Models.IssueTracker;
using Ranger.Core.Models.SourceControl;

namespace Ranger.Core.Linker
{
    public interface IReleaseNoteLinker
    {
        List<ReleaseNoteEntry> Link(List<Commit> commits, List<Issue> issues);
    }
}