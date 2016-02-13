using System;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace ReleaseNoteGenerator.Console
{
    internal class ApplicationBootstrapper : IApplicationBootstrapper
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ApplicationBootstrapper));
        private readonly IConsoleApplication _application;
        private Action _exitOnAction = () => { };

        public ApplicationBootstrapper(IConsoleApplication application)
        {
            _application = application;
        }

        public IApplicationBootstrapper ConfigureLogging()
        {
            var appender = new ColoredConsoleAppender() { Layout = new PatternLayout("%level [%thread] %d{HH:mm:ss} - %message%newline") };
            appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Debug, ForeColor = ColoredConsoleAppender.Colors.Cyan | ColoredConsoleAppender.Colors.HighIntensity });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Info, ForeColor = ColoredConsoleAppender.Colors.Green | ColoredConsoleAppender.Colors.HighIntensity });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Warn, ForeColor = ColoredConsoleAppender.Colors.Purple | ColoredConsoleAppender.Colors.HighIntensity });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Error, ForeColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors { Level = Level.Fatal, ForeColor = ColoredConsoleAppender.Colors.White | ColoredConsoleAppender.Colors.HighIntensity, BackColor = ColoredConsoleAppender.Colors.Red });
            appender.ActivateOptions();

            BasicConfigurator.Configure(appender);

            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.AddAppender(appender);
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;

            return this;
        }

        public void Start(string[] args)
        {
            _application.Run(args).Wait();
            _exitOnAction();
        }

        public IApplicationBootstrapper ExitOn(ConsoleKey key)
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
    }
}