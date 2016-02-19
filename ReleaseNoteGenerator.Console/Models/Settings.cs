using CommandLine;
using CommandLine.Text;

namespace ReleaseNoteGenerator.Console.Models
{
    public class Settings : ApplicationSettings
    {
        [Option('c', "config", Required = true, HelpText = "Config path used to generate release note")]
        public string ConfigPath { get; set; }

        [Option('r', "release", Required = true, HelpText = "Release number")]
        public string ReleaseNumber { get; set; }

        [Option('p', "issue-id-pattern", Required = true, HelpText = "Pattern to extract issue id from commit message")]
        public string IssueIdPattern { get; set; }

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