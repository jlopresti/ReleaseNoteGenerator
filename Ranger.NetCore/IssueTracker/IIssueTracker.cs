using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.NetCore.Models.IssueTracker;

namespace Ranger.NetCore.IssueTracker
{
    public interface IIssueTracker : IRangerPlugin
    {
        Task<List<Issue>> GetIssues(string release);
        Task<Issue> GetIssue(string id);
    }
}