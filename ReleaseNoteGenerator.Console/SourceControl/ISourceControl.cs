using System.Collections.Generic;
using System.Threading.Tasks;
using Ranger.Console.Models.SourceControl;

namespace Ranger.Console.SourceControl
{
    public interface ISourceControl
    {
        Task<List<Commit>> GetCommits(string releaseNumber);
    }
}