using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            return new ApplicationBootstrapper<ReleaseNoteGeneratorConsoleApplication, Settings>()
                .AddDependency<ISourceControlFactory, GithubSourceControlFactory>()
                .AddDependency<IIssueTrackerFactory, JiraIssueTrackerFactory>()
                .ConfigureLogging()
                .ExitOn(ConsoleKey.Enter)
                .Start(args);
        }
    }
}
