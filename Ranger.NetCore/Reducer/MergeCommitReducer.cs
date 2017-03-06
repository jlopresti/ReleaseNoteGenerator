using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Ranger.NetCore.Models.SourceControl;
using Ranger.NetCore.SourceControl;

namespace Ranger.NetCore.Reducer
{
    public class MergeCommitReducer : ICommitReducer
    {
        private readonly ILog _logger;

        public MergeCommitReducer(ILog logger)
        {
            _logger = logger;
        }

        public List<CommitInfo> MergeCommits(List<CommitInfo> result)
        {
            return GetDistinctCommits(result);
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
    }
}