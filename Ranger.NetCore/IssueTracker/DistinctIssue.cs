using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.IssueTracker;

namespace Ranger.NetCore.IssueTracker
{
    public class DistinctIssue : IIssueTracker
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(DistinctIssue));
        private readonly IIssueTracker _innerSourceControl;

        public DistinctIssue(IIssueTracker innerSourceControl)
        {
            Guard.IsNotNull(() => innerSourceControl);

            _innerSourceControl = innerSourceControl;
        }


        public async Task<List<Issue>> GetIssues(string release)
        {
            Guard.IsNotNullOrEmpty(() => release);

            var result = await _innerSourceControl.GetIssues(release);
            _logger.Debug($"[IT] Getting {result.Count} items from issue tracker");
            result = result.Distinct<IReleaseNoteKey>(new ReleaseNoteKeyComparer()).Cast<Issue>().ToList();
            _logger.Debug($"[IT] Getting {result.Count} distincts items from issue tracker after reducing");
            return result;
        }

        public Task<Issue> GetIssue(string id)
        {
            Guard.IsNotNullOrEmpty(() => id);

            _logger.Debug($"[IT] Getting issue {id} from issue tracker");
            return _innerSourceControl.GetIssue(id);
        }

        public void ActivatePlugin()
        {
            _innerSourceControl.ActivatePlugin();
        }
    }
}