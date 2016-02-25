using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Models.SourceControl;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class DistinctCommitSourceControl : ISourceControlProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(DistinctCommitSourceControl));
        private readonly ISourceControlProvider _innerSourceControlProvider;

        public DistinctCommitSourceControl(ISourceControlProvider innerSourceControlProvider)
        {
            Guard.IsNotNull(innerSourceControlProvider);

            _innerSourceControlProvider = innerSourceControlProvider;
        }

        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            Guard.IsNotNullOrEmpty(releaseNumber);

            var result = await _innerSourceControlProvider.GetCommits(releaseNumber);
            _logger.Debug($"[SC] Getting {result.Count} items from source control");
            result = result.Distinct(new ReleaseNoteKeyComparer()).Cast<Commit>().ToList();
            _logger.Debug($"[SC] Getting {result.Count} distincts items from source control after reducing");
            return result;
        }
    }
}