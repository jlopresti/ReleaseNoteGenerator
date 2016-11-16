using System;
using Ranger.Console.Common;
using Ranger.Console.Models;

namespace Ranger.Console
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
