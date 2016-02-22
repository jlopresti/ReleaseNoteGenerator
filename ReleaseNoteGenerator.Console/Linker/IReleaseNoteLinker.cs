using System.Collections.Generic;
using ReleaseNoteGenerator.Console.Models.Binder;
using ReleaseNoteGenerator.Console.Models.IssueTracker;
using ReleaseNoteGenerator.Console.Models.SourceControl;

namespace ReleaseNoteGenerator.Console.Linker
{
    public interface IReleaseNoteLinker
    {
        List<ReleaseNoteEntry> Link(List<Commit> commits, List<Issue> issues);
    }
}