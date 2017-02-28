using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.NetCore.Models.IssueTracker;

namespace Ranger.NetCore.IssueTracker
{
    public interface IIssueTracker
    {
        Task<List<Issue>> GetIssues(string release);
        Task<Issue> GetIssue(string id);
        void ActivatePlugin();
    }
}