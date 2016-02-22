using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Models.SourceControl;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class DistinctCommitSourceControl : ISourceControlProvider
    {
        private readonly ISourceControlProvider _innerSourceControlProvider;

        public DistinctCommitSourceControl(ISourceControlProvider innerSourceControlProvider)
        {
            _innerSourceControlProvider = innerSourceControlProvider;
        }

        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            var result = await _innerSourceControlProvider.GetCommits(releaseNumber);
            result = result.Distinct(new ReleaseNoteKeyComparer()).Cast<Commit>().ToList();
            return result;
        }
    }
}