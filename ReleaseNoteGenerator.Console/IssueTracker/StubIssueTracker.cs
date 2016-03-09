using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atlassian.Jira;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.SourceControl;
using IT = ReleaseNoteGenerator.Console.Models.IssueTracker;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    [Provider("stub")]
    internal class StubIssueTracker : IIssueTracker
    {
        private List<IT.Issue> issues;

        public StubIssueTracker(JObject configPath)
        {
            issues = new List<IT.Issue>();
            issues.Add(new IT.Issue() { Id = "SGR-1", Title = "Hello 1", Type = "Story"});
            issues.Add(new IT.Issue() { Id = "SGR-2", Title = "Hello 2", Type = "Story" });
            issues.Add(new IT.Issue() { Id = "SGR-3", Title = "Hello 3", Type = "Defect" });
            issues.Add(new IT.Issue() { Id = "SGR-4", Title = "Hello 4", Type = "Story" });
            issues.Add(new IT.Issue() { Id = "sdfsd", Title = "Hello 4", Type = "Story" });

            issues[0].AdditionalData.Add("Components", new List<string> {"Project1", "Oracle"});
            issues[1].AdditionalData.Add("Components", new List<string> { "Project2" });
            issues[3].AdditionalData.Add("Components", new List<string> { "Project2", "Oracle" });
        }

        public Task<List<IT.Issue>> GetIssues(string release)
        {
            return Task.FromResult(issues.Where(x => x.Type != "Defect").ToList());
        }

        public IT.Issue GetIssue(string id)
        {
            return issues.FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}