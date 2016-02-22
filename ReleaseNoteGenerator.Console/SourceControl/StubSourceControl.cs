using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Models.SourceControl;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    [Provider("stub")]
    public class StubSourceControl : ISourceControlProvider
    {
        public StubSourceControl(JObject config)
        {
            
        }
        public Task<List<Commit>> GetCommits(string releaseNumber)
        {
            var commits = new List<Commit>();
            commits.Add(new Commit() { Author = "Jilo", Title = "SGR-1 : Hello world 1" });
            commits.Add(new Commit() { Author = "Jilo", Title = "SGR-2 : Hello world 2" });
            commits.Add(new Commit() { Author = "Jilo", Title = "SGR-2 : Hello world 2bis" });
            commits.Add(new Commit() { Author = "Jilo", Title = "SGR-3 : Hello world 3" });
            commits.Add(new Commit() { Author = "Jilo", Title = "Hello world" });
            return Task.FromResult(commits);
        }
    }
}
