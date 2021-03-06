﻿using System;
using System.Threading.Tasks;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Console.Common;
using Ranger.NetCore.Console.Models;
using Ranger.NetCore.Enrichment;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Linker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.Reducer;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.Template;

namespace Ranger.NetCore.Console
{
    internal class ReleaseNoteGeneratorConsoleApplication : IConsoleApplication<ReleaseNoteSettings>
    {
        private ISourceControlPlugin _sourceControlPlugin;
        private IIssueTrackerPlugin _issueTrackerPlugin;
        private ITemplatePlugin _templatePlugin;
        private IPublisherPlugin _publisherPlugin;
        private IReleaseNoteLinker _releaseNoteLinker;
        private readonly IProviderFactory _providerFactory;
        private readonly IReleaseNoteConfiguration _configuration;
        private readonly ILog _logger;
        private ICommitEnrichment _commitEnrichment;
        private ICommitReducer _commitReducer;

        public ReleaseNoteGeneratorConsoleApplication(
            IProviderFactory providerFactory, 
            IReleaseNoteConfiguration configuration, 
            IReleaseNoteLinker releaseNoteLinker,
            ICommitReducer commitReducer,
            ICommitEnrichment commitEnrichment,
            ILog logger)
        {
            _providerFactory = providerFactory;
            _configuration = configuration;
            _releaseNoteLinker = releaseNoteLinker;
            _commitReducer = commitReducer;
            _commitEnrichment = commitEnrichment;
            _logger = logger;
        }

        public async Task<int> Run(ReleaseNoteSettings args)
        {
            _configuration.LoadConfigFile(args.ConfigPath, args.ReleaseNumber);

            _logger.Debug("[APP] Start running application ...");
            _sourceControlPlugin = _providerFactory.CreateSourceControl(_configuration);
            _issueTrackerPlugin = _providerFactory.CreateIssueTracker(_configuration);
            _templatePlugin = _providerFactory.CreateTemplate(_configuration);
            _publisherPlugin = _providerFactory.CreatePublisher(_configuration);
            _commitEnrichment.Setup();

            _logger.Info($"[APP] Retrieving issues for release {_configuration.ReleaseNumber}");
            var issues = await _issueTrackerPlugin.GetIssues(_configuration.ReleaseNumber);

            _logger.Info($"[APP] Retrieving commits for release {_configuration.ReleaseNumber}");
            var commits = await _sourceControlPlugin.GetCommits(_configuration.ReleaseNumber);

            _logger.Info($"[APP] Reduce commits {_configuration.ReleaseNumber}");
            commits = _commitReducer.MergeCommits(commits);

            _logger.Info($"[APP] Enrich commit with issue tracker data");
            commits = await _commitEnrichment.EnrichCommitWithData(commits);

            _logger.Info($"[APP] Start generating model for release {_configuration.ReleaseNumber}");
            var releaseNoteModel = _releaseNoteLinker.Link(commits, issues);

            _logger.Info($"[APP] Start generating release note for release {_configuration.ReleaseNumber}");
            var output = _templatePlugin.Build(_configuration.ReleaseNumber, releaseNoteModel);
            _logger.Debug($"[APP] Release note generated : \n{output}");

            _logger.Info($"[APP] Start publishing for release {_configuration.ReleaseNumber}");
            var result = _publisherPlugin.Publish(_configuration.ReleaseNumber, output);

            var resultCode = result ? Constants.SUCCESS_EXIT_CODE : Constants.FAIL_EXIT_CODE;
            _logger.Debug($"[APP] Process terminated with exit code {resultCode} ...");

            return resultCode;

        }
    }
}