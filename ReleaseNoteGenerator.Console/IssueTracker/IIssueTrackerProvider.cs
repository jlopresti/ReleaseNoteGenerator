using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Models.IssueTracker;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    public interface IIssueTrackerProvider
    {
        Task<List<Issue>> GetIssues(string release);
        Issue GetIssue(string id);
    }
}