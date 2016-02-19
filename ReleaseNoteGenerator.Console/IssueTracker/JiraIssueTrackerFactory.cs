using System;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    class JiraIssueTrackerFactory : IIssueTrackerFactory
    {
        public IIssueTrackerProvider GetProvider(Config settings)
        {
            switch (settings.IssueTrackerType)
            {
                case Common.IssueTracker.Jira:
                    return new JiraIssueTracker(settings.IssueTracker);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}