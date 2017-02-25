using System;

namespace Ranger.NetCore.Console.Common
{
    internal interface IApplicationBootstrapper<TApp, TConfig> 
        where TApp : IConsoleApplication<TConfig> 
        where TConfig : class, IConsoleApplicationConfiguration, new()
    {
        IApplicationBootstrapper<TApp, TConfig> ConfigureLogging();
        int Start(string[] args);
        IApplicationBootstrapper<TApp, TConfig> ExitOn(ConsoleKey key);
        IApplicationBootstrapper<TApp, TConfig> UseDependencyResolver<TModule>() 
            where TModule : IDependencyBootstrapper, new();

        IApplicationBootstrapper<TApp, TConfig> UseConsoleArgsReader<TReader>()
            where TReader : IConsoleArgsReader, new();
    }
}