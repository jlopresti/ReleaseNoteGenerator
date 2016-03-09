using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Models.SourceControl;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public interface ISourceControl
    {
        Task<List<Commit>> GetCommits(string releaseNumber);
    }
}