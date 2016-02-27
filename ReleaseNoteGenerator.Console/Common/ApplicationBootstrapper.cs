using System;
using CommandLine;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Ninject;

namespace ReleaseNoteGenerator.Console.Common
{
    internal class ApplicationBootstrapper<TApp, TParam> : IApplicationBootstrapper<TApp, TParam> where TApp : IConsoleApplication
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ApplicationBootstrapper<TApp, TParam>));
        private Action _exitOnAction = () => { };
        private Hierarchy _hierarchy;
        private readonly IKernel _kernel;

        public ApplicationBootstrapper()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<SettingsWrapper<TParam>>().ToSelf().InTransientScope();

            AddDependency<IConsoleApplication, TApp>();
            AddDependency<TParam, TParam>();
        }

        public IApplicationBootstrapper<TApp, TParam> AddDependency<T, U>()
        {
            _kernel.Bind(typeof(T)).To(typeof(U));
            return this;
        }

        public IApplicationBootstrapper<TApp, TParam> ConfigureLogging()
        {
            var appender = new ColoredConsoleAppender() { Layout = new PatternLayout("%level %d{HH:mm:ss} - %message%newline") };
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
            try
            {
                var settings = _kernel.Get<SettingsWrapper<TParam>>();
                if (Parser.Default.ParseArguments(args, settings.Value))
                {
                    SetupLoggingLevel(settings);
                    var task = _kernel.Get<IConsoleApplication>().Run(args);
                    task.ConfigureAwait(false).GetAwaiter().GetResult();
                    if (task.Result == Constants.SUCCESS_EXIT_CODE && !settings.Silent)
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
                _logger.Error("An unexpected error occurred",ex);
            }

            return Constants.FAIL_EXIT_CODE;
        }

        private void SetupLoggingLevel(SettingsWrapper<TParam> settings)
        {
            if (_hierarchy == null) return;
            if (settings.Verbose)
            {
                _hierarchy.Root.Level = Level.Verbose;
                _logger.Debug("Enable debug mode");
            }
        }

        public IApplicationBootstrapper<TApp, TParam> ExitOn(ConsoleKey key)
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