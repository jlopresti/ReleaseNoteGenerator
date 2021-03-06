﻿using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Ninject;
using Ninject.Modules;

namespace Ranger.Console.Common
{
    internal class ApplicationBootstrapper<TApp, TConfig> : IApplicationBootstrapper<TApp, TConfig> where TApp : IConsoleApplication
        where TConfig : IConsoleApplicationConfiguration, new()
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ApplicationBootstrapper<TApp, TConfig>));
        private Action _exitOnAction = () => { };
        private Hierarchy _hierarchy;
        public static  IKernel _kernel;
        private List<INinjectModule> _modules;

        public ApplicationBootstrapper()
        {
            _modules = new List<INinjectModule>();
            _kernel = NinjectKernel.Instance;
            _kernel.Bind(typeof(IConsoleApplication)).To(typeof(TApp));
        }

        public IApplicationBootstrapper<TApp, TConfig> ConfigureLogging()
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
                var configurationManager = new TConfig();
                if (configurationManager.LoadConfig(args))
                {
                    SetupLoggingLevel(configurationManager);

                    _kernel.Bind<TConfig>().ToMethod(x => configurationManager);
                    if (_modules.Any())
                    {
                        _kernel.Load(_modules);
                    }

                    var application = _kernel.Get<IConsoleApplication>();
                    var task = application.Run(args);
                    task.ConfigureAwait(false).GetAwaiter().GetResult();
                    if (task.Result == Constants.SUCCESS_EXIT_CODE && !configurationManager.Silent)
                        _exitOnAction();
                    application.Dispose();
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

        private void SetupLoggingLevel(TConfig settings)
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

        public ApplicationBootstrapper<TApp, TConfig> WithModule<TModule>() where TModule : INinjectModule, new()
        {
            _modules.Add(new TModule());
            return this;
        }
    }
}