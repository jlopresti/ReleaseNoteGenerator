using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            return new ApplicationBootstrapper<ReleaseNoteGeneratorConsoleApplication, ReleaseNoteConfiguration>()
                .WithModule<ReleaseNoteModule>()
                .ConfigureLogging()
                .ExitOn(ConsoleKey.Enter)
                .Start(args);
        }
    }
}
