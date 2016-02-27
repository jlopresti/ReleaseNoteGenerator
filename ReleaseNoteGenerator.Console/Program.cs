using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Linker;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Publlsher;
using ReleaseNoteGenerator.Console.SourceControl;
using ReleaseNoteGenerator.Console.TemplateProvider;

namespace ReleaseNoteGenerator.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            return new ApplicationBootstrapper<ReleaseNoteGeneratorConsoleApplication, Settings>()
                .AddDependency<IReleaseNoteLinker, ReleaseNoteLinker>()
                .AddDependency<ISourceControlFactory, SourceControlFactory>()
                .AddDependency<IIssueTrackerFactory, JiraIssueTrackerFactory>()
                .AddDependency<ITemplateProviderFactory, TemplateProviderFactory>()
                .AddDependency<IPublisherFactory, PublisherFactory>()
                .ConfigureLogging()
                .ExitOn(ConsoleKey.Enter)
                .Start(args);
        }
    }
}
