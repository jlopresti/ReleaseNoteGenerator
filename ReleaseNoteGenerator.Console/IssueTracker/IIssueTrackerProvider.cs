using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    internal interface IIssueTrackerProvider
    {
        Task<List<Issue>> GetIssues(string release);
    }
}