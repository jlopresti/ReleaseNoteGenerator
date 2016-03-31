using CommandLine;
using CommandLine.Text;
using Ranger.Console.Common;

namespace Ranger.Console.Models
{
    public class Settings
    {
        public Settings()
        {
            GenerateVerb = new GenerateSubSettings();
        }

        [VerbOption("web", HelpText = "Start ranger web server")]
        public WebSubSettings WebVerb { get; set; }

        [VerbOption("generate", HelpText = "Generate release note")]
        public GenerateSubSettings GenerateVerb { get; set; }

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }

    }

    public class WebSubSettings
    {
        [Option('c', "config", Required = true, HelpText = "Config path used to view release notes")]
        public string ConfigPath { get; set; }
    }

    public class GenerateSubSettings : IVerboseParameter, ISilentParameter
    {
        [Option('c', "config", Required = true, HelpText = "Config path used to generate release note")]
        public string ConfigPath { get; set; }

        [Option('r', "release", Required = true, HelpText = "Release number")]
        public string ReleaseNumber { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Enable verbose mode.")]
        public bool Verbose { get; set; }

        [Option('s', "silent", Required = false, HelpText = "Execute in silent mode.")]
        public bool Silent { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        //[HelpOption('h', "help")]
        //public string GetUsage()
        //{
        //    return HelpText.AutoBuild(this,
        //      (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        //}
    }
}