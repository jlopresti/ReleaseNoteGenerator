using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Ranger.Core.Common;
using Issue = Ranger.Core.Models.IssueTracker.Issue;

namespace Ranger.Core.IssueTracker
{
    [Provider("stub")]
    internal class StubIssueTracker : IIssueTracker
    {
        private List<Issue> issues;

        public StubIssueTracker(JObject configPath)
        {
            issues = new List<Issue>();
            issues.Add(new Issue() { Id = "SGR-1", Title = "Hello 1", Type = "Story"});
            issues.Add(new Issue() { Id = "SGR-2", Title = "Hello 2", Type = "Story" });
            issues.Add(new Issue() { Id = "SGR-3", Title = "Hello 3", Type = "Defect" });
            issues.Add(new Issue() { Id = "SGR-4", Title = "Hello 4", Type = "Story" });
            issues.Add(new Issue() { Id = "sdfsd", Title = "Hello 4", Type = "Story" });
            issues.Add(new Issue() { Id = "SGR-54642", Title = "Hello 454654654", Type = "Story" });

            issues[0].AdditionalData.Add("Components", new List<string> {"Project1", "Oracle"});
            issues[1].AdditionalData.Add("Components", new List<string> { "Project2" });
            issues[3].AdditionalData.Add("Components", new List<string> { "Project2", "Oracle" });
            issues[5].AdditionalData.Add("Components", new List<string> { "Project2", "Oracle" });
        }

        public Task<List<Issue>> GetIssues(string release)
        {
            return Task.FromResult(issues.Where(x => x.Type != "Defect").ToList());
        }

        public Issue GetIssue(string id)
        {
            return issues.FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}