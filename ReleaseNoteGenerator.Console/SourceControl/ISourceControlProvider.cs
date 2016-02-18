using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public interface ISourceControlProvider
    {
        Task<List<Commit>> GetCommits(string releaseNumber);
    }
}