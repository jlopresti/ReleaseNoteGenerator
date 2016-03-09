using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Ninject;
using Ninject.Modules;
using Ranger.Console.Common;
using Ranger.Console.IssueTracker;
using Ranger.Console.Linker;
using Ranger.Console.Models;
using Ranger.Console.Publisher;
using Ranger.Console.SourceControl;
using Ranger.Console.TemplateProvider;

namespace Ranger.Console
{
    public class ReleaseNoteModule : NinjectModule
    {
        public override void Load()
        {
            var config = this.Kernel.Get<ReleaseNoteConfiguration>();
            RegisterProviders<IIssueTracker>(config.Config.IssueTracker);
            RegisterProviders<ISourceControl>(config.Config.SourceControl);
            RegisterProviders<ITemplate>(config.Config.Template);
            RegisterProviders<IPublisher>(config.Config.Publish);

            Bind<IReleaseNoteLinker>().To<ReleaseNoteLinker>();
            Bind<ISourceControl>().ToProvider(new SourceControlProvider()).WhenInjectedExactlyInto<EnrichCommitWithIssueTracker>();
            Bind<ISourceControl>().To<EnrichCommitWithIssueTracker>().WhenInjectedExactlyInto<DistinctCommitSourceControl>();
            Bind<ISourceControl>().To<DistinctCommitSourceControl>();
            Bind<IIssueTracker>().ToProvider(new IssueTrackerProvider()).WhenInjectedExactlyInto<DistinctIssue>();
            Bind<IIssueTracker>().To<DistinctIssue>();
            Bind<ITemplate>().ToProvider(new TemplatesProvider());
            Bind<IPublisher>().ToProvider(new PublishProvider());
            
        }

        private void RegisterProviders<T>(object config)
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(p => p.GetCustomAttribute<ProviderAttribute>() != null)
                .ToList();
            foreach (var type1 in types)
            {
                Bind(type1).ToSelf().WithConstructorArgument(typeof(JObject), config);
            }
        }
    }
}