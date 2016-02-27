using System.IO;
using System.Threading.Tasks;
using CommandLine;
using log4net;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Linker;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Publlsher;
using ReleaseNoteGenerator.Console.SourceControl;
using ReleaseNoteGenerator.Console.TemplateProvider;

namespace ReleaseNoteGenerator.Console
{
    internal class ReleaseNoteGeneratorConsoleApplication : IConsoleApplication
    {
        private readonly ISourceControlFactory _sourceControlProvider;
        private readonly IIssueTrackerFactory _issueTrackerFactory;
        private readonly ITemplateProviderFactory _templateProviderFactory;
        private readonly IPublisherFactory _publisherFactory;
        private readonly IReleaseNoteLinker _releaseNoteLinker;
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteGeneratorConsoleApplication));

        public ReleaseNoteGeneratorConsoleApplication(ISourceControlFactory sourceControlFactory, IIssueTrackerFactory issueTrackerFactory,
            ITemplateProviderFactory templateProviderFactory, IPublisherFactory publisherFactory, IReleaseNoteLinker releaseNoteLinker)
        {
            Guard.IsNotNull(() => sourceControlFactory, () => issueTrackerFactory, () => templateProviderFactory, () => publisherFactory, () => releaseNoteLinker);

            _sourceControlProvider = sourceControlFactory;
            _issueTrackerFactory = issueTrackerFactory;
            _templateProviderFactory = templateProviderFactory;
            _publisherFactory = publisherFactory;
            _releaseNoteLinker = releaseNoteLinker;

        }

        public async Task<int> Run(string[] args)
        {
            _logger.Debug("[APP] Start running application ...");
            var settings = new Settings();
            if (Parser.Default.ParseArguments(args, settings))
            {
                Guard.IsValidFilePath(() => settings.ConfigPath);

                _logger.Debug("[APP] Arguments are valid");
                _logger.DebugFormat("[APP] Reading config file at {0}", settings.ConfigPath);
                var config = File.ReadAllText(settings.ConfigPath).ToObject<Config>();

                Guard.IsValidConfig(() => config);

                _logger.Debug("[APP] Getting source control provider");
                var sourceControl = _sourceControlProvider.GetProvider(config);
                _logger.Debug("[APP] Getting issue tracker provider");
                var issueTracker = _issueTrackerFactory.GetProvider(config);
                _logger.Debug("[APP] Getting template provider");
                var templateProvider = _templateProviderFactory.GetProvider(config);
                _logger.Debug("[APP] Getting publisher provider");
                var publisher = _publisherFactory.GetProvider(config);

                _logger.Info($"[APP] Retrieving issues for release {settings.ReleaseNumber}");
                var issues = await issueTracker.GetIssues(settings.ReleaseNumber);
                _logger.Info($"[APP] Retrieving commits for release {settings.ReleaseNumber}");
                var commits = await sourceControl.GetCommits(settings.ReleaseNumber);

                _logger.Info($"[APP] Start generating model for release {settings.ReleaseNumber}");
                var releaseNoteModel = _releaseNoteLinker.Link(commits, issues);
                _logger.Info($"[APP] Start generating release note for release {settings.ReleaseNumber}");
                var output = templateProvider.Build(releaseNoteModel);
                _logger.Debug($"[APP] Release note generated : \n{output}");
                _logger.Info($"[APP] Start publishing for release {settings.ReleaseNumber}");
                var result = publisher.Publish(settings.ReleaseNumber, output);
                var resultCode = result ? Constants.SUCCESS_EXIT_CODE : Constants.FAIL_EXIT_CODE;
                _logger.Debug($"[APP] Process terminated with exit code {resultCode} ...");

                return resultCode;
            }
            _logger.Debug("[APP] Arguments are not valid");
            return Constants.FAIL_EXIT_CODE;
        }
    }
}