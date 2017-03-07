using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.Core.Models.SourceControl;

namespace Ranger.Core.SourceControl
{
    public interface ISourceControl
    {
        Task<List<Commit>> GetCommits(string releaseNumber);
        Task<List<Commit>> GetCommitsFromPastRelease(string release);
    }
}