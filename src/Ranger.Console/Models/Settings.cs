﻿using CommandLine;
using CommandLine.Text;
using Ranger.Console.Common;

namespace Ranger.Console.Models
{
    public class Settings: ISilentParameter, IVerboseParameter
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

        [HelpOption('h', "help")]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

    }
}