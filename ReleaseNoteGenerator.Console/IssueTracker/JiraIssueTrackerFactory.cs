using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    class JiraIssueTrackerFactory : IIssueTrackerFactory
    {
        public IIssueTrackerProvider GetProvider(JObject settings)
        {
            return new JiraIssueTracker(settings.ConfigPath);
        }
    }
}