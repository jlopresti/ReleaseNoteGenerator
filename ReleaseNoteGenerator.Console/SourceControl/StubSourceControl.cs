using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models.SourceControl;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    [Provider("stub")]
    public class StubSourceControl : ISourceControl
    {
        public StubSourceControl(JObject config)
        {
            
        }
        public async Task<List<Commit>> GetCommits(string releaseNumber)
        {
            var commits = new List<Commit>();
            commits.Add(new Commit() { Authors = new List<string> { "Jilo1" }, Title = "CLOUD-8679 : Hello world 1" });
            commits.Add(new Commit() { Authors =new List<string> {  "Jilo2"}, Title = "SGR-2 : Hello world 2" });
            commits.Add(new Commit() { Authors =new List<string> {  "Jilo3"}, Title = "SGR-2 : Hello world 2bis" });
            commits.Add(new Commit() { Authors =new List<string> {  "Jilo4"}, Title = "SGR-3 : Hello world 3" });
            commits.Add(new Commit() { Authors = new List<string> { "Jilo5"}, Title = "Hello world" });
            await Task.Delay(5000);
            return commits;
        }
    }
}
