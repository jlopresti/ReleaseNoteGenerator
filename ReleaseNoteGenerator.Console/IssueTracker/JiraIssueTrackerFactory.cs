using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    class JiraIssueTrackerFactory : IIssueTrackerFactory
    {
        public IIssueTrackerProvider GetProvider(Settings settings)
        {
            return new JiraIssueTracker(settings.IssueTrackerConfigPath);
        }
    }
}