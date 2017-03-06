using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using log4net;
using log4net.Plugin;
using Ranger.NetCore.Common;
using Ranger.NetCore.Console.Common;
using Ranger.NetCore.Console.Models;
using Ranger.NetCore.Enrichment;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Linker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.Reducer;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.Template;
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

            _container.RegisterSingleton<IReleaseNoteConfiguration, ReleaseNoteConfiguration>();
            _container.RegisterSingleton<IDependencyResolver>(this);

            string pluginDirectory = Path.Combine(AppContext.BaseDirectory);
            var pluginAssemblies =
                (from file in new DirectoryInfo(pluginDirectory).GetFiles()
                 where file.Extension.ToLower() == ".dll" && file.Name != ("Ranger.NetCore.Console.dll")
                 select AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName))
                .ToList();

            
            _container.RegisterCollection<IIssueTrackerPlugin>(pluginAssemblies);
            _container.RegisterCollection<ISourceControlPlugin>(pluginAssemblies);
            _container.RegisterCollection<IPublisherPlugin>(pluginAssemblies);
            _container.RegisterCollection<ITemplatePlugin>(pluginAssemblies);

            _container.RegisterSingleton<IProviderFactory, ProviderFactory>();
            _container.RegisterSingleton<IReleaseNoteLinker, ReleaseNoteLinker>();
            _container.RegisterSingleton<ICommitReducer, MergeCommitReducer>();
            _container.RegisterSingleton<ICommitEnrichment, EnrichCommitWithIssueTracker>();

            _container.RegisterConditional(typeof(ILog),
                c => typeof(Log4NetAdapter<>).MakeGenericType(c.Consumer.ImplementationType),
                Lifestyle.Singleton,
                c => true);

            _container.Verify();

            return this;
        }
    }
}
