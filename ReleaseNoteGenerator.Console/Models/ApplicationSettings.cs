using CommandLine;

namespace ReleaseNoteGenerator.Console.Models
{
    public class ApplicationSettings
    {
        [Option('v', "verbose", Required = false, HelpText = "Enable verbose mode.")]
        public bool Verbose { get; set; }
    }
}