using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Ninject;
using Ninject.Modules;
using Ranger.Console.Models;
using Ranger.Core.Common;
using Ranger.Core.IssueTracker;
using Ranger.Core.Linker;
using Ranger.Core.Models;
using Ranger.Core.Publisher;
using Ranger.Core.SourceControl;
using Ranger.Core.TemplateProvider;

namespace Ranger.Console
{
    public class ReleaseNoteModule : NinjectModule
    {
        public override void Load()
        {
            var config = this.Kernel.Get<ReleaseNoteConfiguration>();

            this.Kernel.Bind<IReleaseNoteConfiguration>().ToMethod(x => config);

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