using CommandLine;
using CommandLine.Text;

namespace ReleaseNoteGenerator.Console.Models
{
    public class Settings : ApplicationSettings
    {
        [Option('s', "sc-config", Required = true, HelpText = "Source Control config path for creating release note")]
        public string SourceControlConfigPath { get; set; }

        [Option('i', "it-config", Required = true, HelpText = "Issue Tracker config path for creating release note")]
        public string IssueTrackerConfigPath { get; set; }

        [Option('p', "publish", Required = false, DefaultValue = false, HelpText = "Jira config path for creating release note")]
        public bool Publish { get; set; }

        [Option('r', "release", Required = true, HelpText = "Release number")]
        public bool RelNumber { get; set; }

        [Option('c', "issue-commit-pattern", Required = true, HelpText = "Release number")]
        public string IssueCommitPattern { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption('h', "help")]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

    }
}