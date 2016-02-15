using System.Threading.Tasks;
using CommandLine;
using log4net;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.Common
{
    internal class ReleaseNoteGeneratorConsoleApplication : IConsoleApplication
    {
        private readonly ISourceControlFactory _sourceControlProvider;
        private readonly IIssueTrackerFactory _issueTrackerFactory;
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteGeneratorConsoleApplication));

        public ReleaseNoteGeneratorConsoleApplication(ISourceControlFactory sourceControlFactory, IIssueTrackerFactory issueTrackerFactory)
        {
            _sourceControlProvider = sourceControlFactory;
            _issueTrackerFactory = issueTrackerFactory;
        }

        public Task<int> Run(string[] args)
        {
            var settings = new Settings();
            if (Parser.Default.ParseArguments(args, settings))
            {
                var sourceControl = _sourceControlProvider.GetProvider(settings);
                var issueTracker = _issueTrackerFactory.GetProvider(settings);              
            }
            return Task.FromResult(Constants.FAIL_EXIT_CODE);
        }
    }
}