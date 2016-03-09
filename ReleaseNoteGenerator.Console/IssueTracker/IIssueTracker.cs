using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.Console.Models.IssueTracker;

namespace Ranger.Console.IssueTracker
{
    public interface IIssueTracker
    {
        Task<List<Issue>> GetIssues(string release);
        Issue GetIssue(string id);
    }
}