using System;

namespace ReleaseNoteGenerator.Console.Common
{
    internal interface IApplicationBootstrapper
    {
        IApplicationBootstrapper ConfigureLogging();
        int Start(string[] args);
        IApplicationBootstrapper ExitOn(ConsoleKey key);
    }
}