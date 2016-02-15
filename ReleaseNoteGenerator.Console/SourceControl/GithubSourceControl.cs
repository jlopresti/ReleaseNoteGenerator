namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class GithubSourceControl : ISourceControlProvider
    {
        private string _config;

        public GithubSourceControl(string config)
        {
            _config = config;
        }
    }
}