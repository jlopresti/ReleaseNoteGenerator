using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Models.IssueTracker;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    public class DistinctIssueProvider : IIssueTrackerProvider
    {
        private readonly IIssueTrackerProvider _innerSourceControlProvider;

        public DistinctIssueProvider(IIssueTrackerProvider innerSourceControlProvider)
        {
            _innerSourceControlProvider = innerSourceControlProvider;
        }


        public async Task<List<Issue>> GetIssues(string release)
        {
            var result = await _innerSourceControlProvider.GetIssues(release);
            result = result.Distinct<IReleaseNoteKey>(new ReleaseNoteKeyComparer()).Cast<Issue>().ToList();
            return result;
        }

        public Issue GetIssue(string id)
        {
            return _innerSourceControlProvider.GetIssue(id);
        }
    }
}