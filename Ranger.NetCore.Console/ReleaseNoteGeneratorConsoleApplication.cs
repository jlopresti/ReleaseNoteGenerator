using System;
using System.Threading.Tasks;
using log4net;
using Ranger.NetCore.Console.Common;
using Ranger.NetCore.Console.Models;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Linker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.TemplateProvider;

namespace Ranger.NetCore.Console
{
    internal class ReleaseNoteGeneratorConsoleApplication : IConsoleApplication<ReleaseNoteSettings>
    {
        private ISourceControl _sourceControl;
        private IIssueTracker _issueTracker;
        private ITemplate _template;
        private IPublisher _publisher;
        private IReleaseNoteLinker _releaseNoteLinker;
        private readonly IProviderFactory _providerFactory;
        private readonly IReleaseNoteConfiguration _configuration;
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteGeneratorConsoleApplication));

        public ReleaseNoteGeneratorConsoleApplication(IProviderFactory providerFactory, 
            IReleaseNoteConfiguration configuration)
        {
            _providerFactory = providerFactory;
            _configuration = configuration;
        }

        public async Task<int> Run(ReleaseNoteSettings args)
        {
            _logger.Debug("[APP] Start running application ...");
            _sourceControl = _providerFactory.CreateSourceControl(_configuration);
            _issueTracker = _providerFactory.CreateIssueTracker(_configuration);
            _template = _providerFactory.CreateTemplate(_configuration);
            _publisher = _providerFactory.CreatePublisher(_configuration);
            _releaseNoteLinker = _providerFactory.CreateReleaseNoteLinker();

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