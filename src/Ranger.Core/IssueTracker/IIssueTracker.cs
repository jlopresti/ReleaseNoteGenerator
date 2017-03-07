using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.Core.Models.IssueTracker;

namespace Ranger.Core.IssueTracker
{
    public interface IIssueTracker
    {
        Task<List<Issue>> GetIssues(string release);
        Issue GetIssue(string id);
    }
}