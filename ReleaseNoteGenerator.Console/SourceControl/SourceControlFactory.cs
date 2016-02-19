using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class GithubSourceControlFactory : ISourceControlFactory
    {
        public ISourceControlProvider GetProvider(JObject settings)
        {
            return new GithubSourceControl(settings.ConfigPath);
        }
    }
}