using System;
using Ninject.Modules;

namespace Ranger.Console.Common
{
    internal interface IApplicationBootstrapper<TApp, TConfig> where TApp : IConsoleApplication 
        where TConfig : IConsoleApplicationConfiguration, new()
    {
        IApplicationBootstrapper<TApp, TConfig> ConfigureLogging();
        int Start(string[] args);
        IApplicationBootstrapper<TApp, TConfig> ExitOn(ConsoleKey key);
        ApplicationBootstrapper<TApp, TConfig> WithModule<TModule>() where TModule : INinjectModule, new();
    }
}