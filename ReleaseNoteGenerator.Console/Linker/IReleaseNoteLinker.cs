using System.Collections.Generic;
using Ranger.Console.Models.Binder;
using Ranger.Console.Models.IssueTracker;
using Ranger.Console.Models.SourceControl;

namespace Ranger.Console.Linker
{
    public interface IReleaseNoteLinker
    {
        List<ReleaseNoteEntry> Link(List<Commit> commits, List<Issue> issues);
    }
}