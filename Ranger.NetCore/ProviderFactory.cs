using System;
using System.Linq;
using System.Reflection;
using Ranger.NetCore.Common;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Linker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.TemplateProvider;

namespace Ranger.NetCore
{
    public class ProviderFactory : IProviderFactory
    {
        private readonly IDependencyResolver _dependencyResolver;

        public ProviderFactory(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }
        public ISourceControl CreateSourceControl(IReleaseNoteConfiguration wrapper)
        {
            var t = _dependencyResolver.ResolveAll
                <ISourceControl>().SingleOrDefault(
                x => x.GetType().GetTypeInfo()
                    .GetCustomAttribute<ProviderAttribute>()
                    .Name.Equals(wrapper.GetSourceControlConfig<BasePluginConfig>().Provider,
                        StringComparison.CurrentCultureIgnoreCase));

            t?.ActivatePlugin();
            return t;
        }

        public IIssueTracker CreateIssueTracker(IReleaseNoteConfiguration wrapper)
        {
            var t = _dependencyResolver.ResolveAll<IIssueTracker>().SingleOrDefault(
                x => x.GetType().GetTypeInfo()
                    .GetCustomAttribute<ProviderAttribute>()
                    .Name.Equals(wrapper.GetIssueTrackerConfig<BasePluginConfig>().Provider,
                        StringComparison.CurrentCultureIgnoreCase));
            
            t?.ActivatePlugin();
            return t;
        }

        public IPublisher CreatePublisher(IReleaseNoteConfiguration wrapper)
        {
            var t = _dependencyResolver.ResolveAll<IPublisher>().SingleOrDefault(
               x => x.GetType().GetTypeInfo()
                   .GetCustomAttribute<ProviderAttribute>()
                   .Name.Equals(wrapper.GetPublisherConfig<BasePluginConfig>().Provider,
                       StringComparison.CurrentCultureIgnoreCase));
            t?.ActivatePlugin();
            return t;
        }

        public ITemplate CreateTemplate(IReleaseNoteConfiguration wrapper)
        {
            var t = _dependencyResolver.ResolveAll<ITemplate>().SingleOrDefault(
                x => x.GetType().GetTypeInfo()
                    .GetCustomAttribute<ProviderAttribute>()
                    .Name.Equals(wrapper.GetTemplateConfig<BasePluginConfig>().Provider,
                        StringComparison.CurrentCultureIgnoreCase));
            t?.ActivatePlugin();
            return t;
        }

        public IReleaseNoteLinker CreateReleaseNoteLinker()
        {
            return new ReleaseNoteLinker();
        }
    }
}