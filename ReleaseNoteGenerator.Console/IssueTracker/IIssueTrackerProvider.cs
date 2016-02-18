using System.Collections.Generic;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    internal interface IIssueTrackerProvider
    {
        List<Issue> GetIssues(string release);
    }
}