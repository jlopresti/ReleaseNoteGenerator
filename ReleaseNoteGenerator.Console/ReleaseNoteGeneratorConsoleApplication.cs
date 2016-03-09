using System.IO;
using System.Threading.Tasks;
using CommandLine;
using log4net;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Linker;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Publisher;
using ReleaseNoteGenerator.Console.SourceControl;
using ReleaseNoteGenerator.Console.TemplateProvider;

namespace ReleaseNoteGenerator.Console
{
    internal class ReleaseNoteGeneratorConsoleApplication : IConsoleApplication
    {
        private readonly ISourceControl _sourceControl;
        private readonly IIssueTracker _issueTracker;
        private readonly ITemplate _template;
        private readonly IPublisher _publisher;
        private readonly IReleaseNoteLinker _releaseNoteLinker;
        private readonly ReleaseNoteConfiguration _configuration;
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteGeneratorConsoleApplication));

        public ReleaseNoteGeneratorConsoleApplication(ISourceControl sourceControl, IIssueTracker issueTracker,
            ITemplate template, IPublisher publisher, IReleaseNoteLinker releaseNoteLinker, ReleaseNoteConfiguration configuration)
        {
            Guard.IsNotNull(() => sourceControl, () => issueTracker, () => template, () => publisher, () => releaseNoteLinker);

            _sourceControl = sourceControl;
            _issueTracker = issueTracker;
            _template = template;
            _publisher = publisher;
            _releaseNoteLinker = releaseNoteLinker;
            _configuration = configuration;
        }

        public async Task<int> Run(string[] args)
        {
            _logger.Debug("[APP] Start running application ...");

            _logger.Info($"[APP] Retrieving issues for release {_configuration.ReleaseNumber}");
            var issues = await _issueTracker.GetIssues(_configuration.ReleaseNumber);

            _logger.Info($"[APP] Retrieving commits for release {_configuration.ReleaseNumber}");
            var commits = await _sourceControl.GetCommits(_configuration.ReleaseNumber);

            _logger.Info($"[APP] Start generating model for release {_configuration.ReleaseNumber}");
            var releaseNoteModel = _releaseNoteLinker.Link(commits, issues);

            _logger.Info($"[APP] Start generating release note for release {_configuration.ReleaseNumber}");
            var output = _template.Build(_configuration.ReleaseNumber, releaseNoteModel);
            _logger.Debug($"[APP] Release note generated : \n{output}");

            _logger.Info($"[APP] Start publishing for release {_configuration.ReleaseNumber}");
            var result = _publisher.Publish(_configuration.ReleaseNumber, output);

            var resultCode = result ? Constants.SUCCESS_EXIT_CODE : Constants.FAIL_EXIT_CODE;
            _logger.Debug($"[APP] Process terminated with exit code {resultCode} ...");

            return resultCode;

        }
    }
}