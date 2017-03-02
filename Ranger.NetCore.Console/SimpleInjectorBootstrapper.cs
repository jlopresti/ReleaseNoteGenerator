using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using log4net.Plugin;
using Ranger.NetCore.Console.Common;
using Ranger.NetCore.Console.Models;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Linker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.TemplateProvider;
using SimpleInjector;

namespace Ranger.NetCore.Console
{
    public class SimpleInjectorBootstrapper : IDependencyResolver, IDependencyBootstrapper
    {
        private Container _container;


        public T Resolve<T>() where T : class
        {
            return _container.GetInstance<T>();
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return _container.GetAllInstances<T>();
        }

        public IDependencyResolver Configure(Type application)
        {
            _container = new Container();

            _container.RegisterSingleton(typeof(IConsoleApplication<>), application);
            //_container.RegisterSingleton(typeof(IConsoleApplicationConfiguration), configuration);
            _container.RegisterSingleton<IReleaseNoteConfiguration, ReleaseNoteConfiguration>();
            _container.RegisterSingleton<IDependencyResolver>(this);

            string pluginDirectory =
    Path.Combine(AppContext.BaseDirectory);

            var pluginAssemblies =
                (from file in new DirectoryInfo(pluginDirectory).GetFiles()
                 where file.Extension.ToLower() == ".dll"
                 select Assembly.Load(AssemblyLoadContext.GetAssemblyName(file.FullName)))
                .Where(x => x.FullName.StartsWith("Ranger.NetCore"))
                .ToList();

            _container.RegisterCollection<IIssueTracker>(pluginAssemblies);
            _container.RegisterCollection<ISourceControl>(pluginAssemblies);
            _container.RegisterCollection<IPublisher>(pluginAssemblies);
            _container.RegisterCollection<ITemplate>(pluginAssemblies);

            _container.RegisterSingleton<IProviderFactory, ProviderFactory>();

            _container.Verify();

            return this;
        }
    }
}
