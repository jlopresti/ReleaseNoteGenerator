using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    internal interface IIssueTrackerFactory
    {
        IIssueTrackerProvider GetProvider(Config settings);
    }
}