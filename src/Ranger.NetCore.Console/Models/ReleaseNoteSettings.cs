using CommandLine;
using CommandLine.Text;
using Ranger.NetCore.Console.Common;

namespace Ranger.NetCore.Console.Models
{
    public class ReleaseNoteSettings: IConsoleApplicationConfiguration
    {
        [Option('c', "config", Required = true, HelpText = "Config path used to generate release note")]
        public string ConfigPath { get; set; }

        [Option('r', "release", Required = true, HelpText = "Release number")]
        public string ReleaseNumber { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Enable verbose mode.")]
        public bool Verbose { get; set; }

        [Option('s', "silent", Required = false, HelpText = "Execute in silent mode.")]
        public bool Silent { get; set; }

    }
}