using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class GithubSourceControlFactory : ISourceControlFactory
    {
        public ISourceControlProvider GetProvider(Settings settings)
        {
            return new GithubSourceControl(settings.SourceControlConfigPath);
        }
    }
}