using System;
using CommandLine;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    internal class ApplicationBootstrapper : IApplicationBootstrapper
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ApplicationBootstrapper));
        private readonly IConsoleApplication _application;
        private Action _exitOnAction = () => { };
        private Hierarchy _hierarchy;

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

            _hierarchy = (Hierarchy)LogManager.GetRepository();
            _hierarchy.Root.AddAppender(appender);
            _hierarchy.Root.Level = Level.Info;
            _hierarchy.Configured = true;

            return this;
        }

        public int Start(string[] args)
        {
            var settings = new ApplicationSettings();
            if (Parser.Default.ParseArguments(args, settings))
            {
                SetupLoggingLevel(settings);
                var task = _application.Run(args);
                task.Wait();
                if(task.Result == Constants.SUCCESS_EXIT_CODE)
                _exitOnAction();
                return task.Result;
            }
            return Constants.FAIL_EXIT_CODE;
        }

        private void SetupLoggingLevel(ApplicationSettings settings)
        {
            if (_hierarchy == null) return;
            if (settings.Verbose) _hierarchy.Root.Level = Level.Verbose;
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