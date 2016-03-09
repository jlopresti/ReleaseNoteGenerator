using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Ranger.Console.Helpers;
using Ranger.Console.Models.SourceControl;

namespace Ranger.Console.SourceControl
{
    public class DistinctCommitSourceControl : ISourceControl
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(DistinctCommitSourceControl));
        private readonly ISourceControl _innerSourceControl;

        public DistinctCommitSourceControl(ISourceControl innerSourceControl)
        {
            Guard.IsNotNull(() => innerSourceControl);

            _innerSourceControl = innerSourceControl;
        }

        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            Guard.IsNotNullOrEmpty(() => releaseNumber);

            var result = await _innerSourceControl.GetCommits(releaseNumber);
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