namespace ReleaseNoteGenerator.Console.IssueTracker
{
    internal class JiraIssueTracker : IIssueTrackerProvider
    {
        private readonly string _config;

        public JiraIssueTracker(string config)
        {
            _config = config;
        }
    }
}