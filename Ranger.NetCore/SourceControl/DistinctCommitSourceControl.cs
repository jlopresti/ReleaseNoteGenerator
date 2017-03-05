using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.SourceControl
{
    public class DistinctCommitSourceControl : ISourceControl
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(DistinctCommitSourceControl));
        private readonly ISourceControl _innerSourceControl;

        public DistinctCommitSourceControl(ISourceControl innerSourceControl)
        {

            _innerSourceControl = innerSourceControl;
        }

        public async Task<List<CommitInfo>> GetCommits(string releaseNumber)
        {

            var result = await _innerSourceControl.GetCommits(releaseNumber);
            result = GetDistinctCommits(result);
            return result;
        }

        private List<CommitInfo> GetDistinctCommits(List<CommitInfo> result)
        {
            _logger.Debug($"[SC] Getting {result.Count} items from source control");
            result = result.GroupBy(x => x.Id).Select(x =>
            {
                var c = x.First();
                c.Authors = x.SelectMany(_ => _.Authors).Distinct().ToList();
                return c;
            }).ToList();
            _logger.Debug($"[SC] Getting {result.Count} distincts items from source control after reducing");
            return result;
        }

        public async Task<List<CommitInfo>> GetCommitsFromPastRelease(string release)
        {          

            var result = await _innerSourceControl.GetCommitsFromPastRelease(release);
            result = GetDistinctCommits(result);
            return result;
        }

        public void ActivatePlugin()
        {
            _innerSourceControl.ActivatePlugin();
        }

        public string PluginId => _innerSourceControl.PluginId;
    }
}