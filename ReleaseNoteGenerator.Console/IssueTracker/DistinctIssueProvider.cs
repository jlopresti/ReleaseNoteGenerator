using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Models.IssueTracker;

namespace ReleaseNoteGenerator.Console.IssueTracker
{
    public class DistinctIssueProvider : IIssueTrackerProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(DistinctIssueProvider));
        private readonly IIssueTrackerProvider _innerSourceControlProvider;

        public DistinctIssueProvider(IIssueTrackerProvider innerSourceControlProvider)
        {
            Guard.IsNotNull(innerSourceControlProvider);

            _innerSourceControlProvider = innerSourceControlProvider;
        }


        public async Task<List<Issue>> GetIssues(string release)
        {
            Guard.IsNotNullOrEmpty(release);

            var result = await _innerSourceControlProvider.GetIssues(release);
            _logger.Debug($"[IT] Getting {result.Count} items from issue tracker");
            result = result.Distinct<IReleaseNoteKey>(new ReleaseNoteKeyComparer()).Cast<Issue>().ToList();
            _logger.Debug($"[IT] Getting {result.Count} distincts items from issue tracker after reducing");
            return result;
        }

        public Issue GetIssue(string id)
        {
            Guard.IsNotNullOrEmpty(id);

            _logger.Debug($"[IT] Getting issue {id} from issue tracker");
            return _innerSourceControlProvider.GetIssue(id);
        }
    }
}