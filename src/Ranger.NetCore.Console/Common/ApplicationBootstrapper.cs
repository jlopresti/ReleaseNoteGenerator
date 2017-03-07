using System;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Ranger.NetCore.Common;

namespace Ranger.NetCore.Console.Common
{
    internal class ApplicationBootstrapper<TApp, TConfig> : IApplicationBootstrapper<TApp, TConfig> 
        where TApp : IConsoleApplication<TConfig>
        where TConfig : class, new()
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ApplicationBootstrapper<TApp, TConfig>));
        private Action _exitOnAction = () => { };
        private Hierarchy _hierarchy;
        private IDependencyResolver _dependencyResolver;
        private IConsoleArgsReader _reader;

        public ApplicationBootstrapper()
        {
        }

        public IApplicationBootstrapper<TApp, TConfig> ConfigureLogging()
        {
            var appender = new ConsoleAppender() { Layout = new PatternLayout("%level %d{HH:mm:ss} - %message%newline") };
            //appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Debug, ForeColor = ColoredConsoleAppender.Colors.Cyan | ColoredConsoleAppender.Colors.HighIntensity });
            //appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Info, ForeColor = ColoredConsoleAppender.Colors.Green | ColoredConsoleAppender.Colors.HighIntensity });
            //appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Warn, ForeColor = ColoredConsoleAppender.Colors.Purple | ColoredConsoleAppender.Colors.HighIntensity });
            //appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Error, ForeColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity });
            //appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Fatal, ForeColor = ColoredConsoleAppender.Colors.White | ColoredConsoleAppender.Colors.HighIntensity, BackColor = ColoredConsoleAppender.Colors.Red });
            appender.ActivateOptions();

            //BasicConfigurator.Configure(appender);

            _hierarchy = (Hierarchy)LogManager.GetRepository(Assembly.GetEntryAssembly());
            _hierarchy.Root.AddAppender(appender);
            _hierarchy.Root.Level = Level.Info;
            _hierarchy.Configured = true;

            return this;
        }

        public int Start(string[] args)
        {
            try
            {
                var consoleParams = _reader.ReadConsoleArgs<TConfig>(args);
                if (consoleParams.Success)
                {
                    SetupLoggingLevel(consoleParams.Parameters);

                    var application = _dependencyResolver.Resolve<IConsoleApplication<TConfig>>();
                    var task = application.Run(consoleParams.Parameters.Value);
                    task.ConfigureAwait(false).GetAwaiter().GetResult();
                    if (task.Result == Constants.SUCCESS_EXIT_CODE && !consoleParams.Parameters.Silent)
                        _exitOnAction();
                    
                    return task.Result;
                }
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex.Message);   
            }
            catch (Exception ex)
            {
                _logger.Error("An unexpected error occurred", ex);
            }

            return Constants.FAIL_EXIT_CODE;
        }

        private void SetupLoggingLevel(IConsoleApplicationConfiguration settings)
        {
            if (_hierarchy == null) return;
            if (settings.Verbose)
            {
                _hierarchy.Root.Level = Level.Verbose;
                _logger.Debug("Enable debug mode");
            }
        }

        public IApplicationBootstrapper<TApp, TConfig> ExitOn(ConsoleKey key)
        {
            _exitOnAction = () =>
            {
                _logger.Info($"Press '{key}' to Exit");
                while (System.Console.ReadKey(true).Key != key)
                {
                }
            };
            return this;
        }

        public IApplicationBootstrapper<TApp, TConfig> UseDependencyResolver<TBootstrapper>() 
            where TBootstrapper : IDependencyBootstrapper, new()
        {
            var bootstrapper = new TBootstrapper();
            _dependencyResolver = bootstrapper.Configure(typeof(TApp));
            return this;
        }

        public IApplicationBootstrapper<TApp, TConfig> UseConsoleArgsReader<TReader>()
            where TReader : IConsoleArgsReader, new()
        {
            _reader = new TReader();
            return this;
        }
    }
}