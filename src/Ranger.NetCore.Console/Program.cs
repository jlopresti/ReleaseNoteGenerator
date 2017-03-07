using System;
using Ranger.NetCore.Console.Common;
using Ranger.NetCore.Console.Models;

namespace Ranger.NetCore.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            return new ApplicationBootstrapper<ReleaseNoteGeneratorConsoleApplication, ReleaseNoteSettings>()
                .UseDependencyResolver<SimpleInjectorBootstrapper>()
                .UseConsoleArgsReader<SimpleConsoleArgsReader>()
                .ConfigureLogging()
                .ExitOn(ConsoleKey.Enter)
                .Start(args);
        }
    }
}