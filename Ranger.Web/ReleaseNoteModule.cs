using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Ninject;
using Ninject.Modules;
using Ranger.Core.Common;
using Ranger.Core.IssueTracker;
using Ranger.Core.Linker;
using Ranger.Core.Models;
using Ranger.Core.Models.Template;
using Ranger.Core.Publisher;
using Ranger.Core.SourceControl;
using Ranger.Core.TemplateProvider;
using Ranger.Web.Models;
using Ranger.Web.Models.Home;
using Ranger.Web.Models.TemplateProvider;

namespace Ranger.Web
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
            Bind<ITemplate>().To<WebHtmlFileTemplate>().WithConstructorArgument(typeof(HtmlFileTemplateConfig), config.Config.Template.ToObject<HtmlFileTemplateConfig>());
            Bind<IPublisher>().ToProvider(new PublishProvider());
            
        }

        private void RegisterProviders<T>(JObject config)
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(p => p.GetCustomAttribute<ProviderAttribute>() != null)
                .ToList();
            foreach (var type1 in types)
            {
                var attr = type1.GetCustomAttribute<ProviderAttribute>();
                if (attr.ConfigurationType == null)
                {
                    Bind(type1).ToSelf().WithConstructorArgument(typeof(JObject), config);
                }
                else
                {

                    Bind(type1).ToSelf().WithConstructorArgument(attr.ConfigurationType, config.ToObject(attr.ConfigurationType));
                }
            }
        }
    }
}