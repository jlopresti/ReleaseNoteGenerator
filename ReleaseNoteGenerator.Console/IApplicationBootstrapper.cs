using System;

namespace ReleaseNoteGenerator.Console
{
    internal interface IApplicationBootstrapper
    {
        IApplicationBootstrapper ConfigureLogging();
        void Start(string[] args);
        IApplicationBootstrapper ExitOn(ConsoleKey key);
    }
}