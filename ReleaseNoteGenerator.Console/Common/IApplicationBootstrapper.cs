using System;

namespace ReleaseNoteGenerator.Console.Common
{
    internal interface IApplicationBootstrapper<TApp, TParam> where TApp : IConsoleApplication
    {
        IApplicationBootstrapper<TApp, TParam> ConfigureLogging();
        int Start(string[] args);
        IApplicationBootstrapper<TApp, TParam> ExitOn(ConsoleKey key);
        IApplicationBootstrapper<TApp, TParam> AddDependency<T, U>();
    }
}