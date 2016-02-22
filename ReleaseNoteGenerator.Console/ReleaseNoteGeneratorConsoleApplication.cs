using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using log4net;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Linker;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Models.IssueTracker;
using ReleaseNoteGenerator.Console.Models.SourceControl;
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
            _sourceControlProvider = sourceControlFactory;
            _issueTrackerFactory = issueTrackerFactory;
            _templateProviderFactory = templateProviderFactory;
            _publisherFactory = publisherFactory;
            _releaseNoteLinker = releaseNoteLinker;
          
        }

        public async Task<int> Run(string[] args)
        {
            _logger.Debug("Start running application ...");
            var settings = new Settings();
            if (Parser.Default.ParseArguments(args, settings))
            {
                _logger.Debug("Arguments are valid");
                _logger.DebugFormat("Reading config file at {0}", settings.ConfigPath);
                var config = File.ReadAllText(settings.ConfigPath).ToObject<Config>();
                var sourceControl = _sourceControlProvider.GetProvider(config);
                var issueTracker = _issueTrackerFactory.GetProvider(config);
                var templateProvider = _templateProviderFactory.GetProvider(config);
                var publisher = _publisherFactory.GetProvider(config);
                var issues = await issueTracker.GetIssues(settings.ReleaseNumber);
                var commits = await sourceControl.GetCommits(settings.ReleaseNumber);
                var releaseNoteModel = _releaseNoteLinker.Link(commits, issues);
                var output = templateProvider.Build(releaseNoteModel);
                var result = publisher.Publish(output);
                _logger.Debug("Process terminated ...");
                return result ? Constants.SUCCESS_EXIT_CODE : Constants.FAIL_EXIT_CODE;
            }
            _logger.Debug("Arguments are not valid");
            return Constants.FAIL_EXIT_CODE;
        }       
    }
}