using System;
using System.Linq;
using System.Reflection;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
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
            var providerName = wrapper.GetSourceControlConfig<BasePluginConfig>().Provider;

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ApplicationException("Source control provider name is not specified");
            }

            var plugin = _dependencyResolver.ResolveAll<ISourceControl>().GetPlugins(providerName);

            if (plugin == null)
            {
                throw new ApplicationException($"No source control plugin found with name {providerName}");
            }

            plugin.ActivatePlugin();
            return plugin;
        }

        public IIssueTracker CreateIssueTracker(IReleaseNoteConfiguration wrapper)
        {
            var providerName = wrapper.GetIssueTrackerConfig<BasePluginConfig>().Provider;

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ApplicationException("Issue tracker provider name is not specified");
            }

            var plugin = _dependencyResolver.ResolveAll<IIssueTracker>().GetPlugins(providerName);

            if (plugin == null)
            {
                throw new ApplicationException($"No issue tracker plugin found with name {providerName}");
            }

            plugin.ActivatePlugin();
            return plugin;
        }

        public IPublisher CreatePublisher(IReleaseNoteConfiguration wrapper)
        {
            var providerName = wrapper.GetPublisherConfig<BasePluginConfig>().Provider;

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ApplicationException("Publisher provider name is not specified");
            }

            var plugin = _dependencyResolver.ResolveAll<IPublisher>().GetPlugins(providerName);

            if (plugin == null)
            {
                throw new ApplicationException($"No publisher plugin found with name {providerName}");
            }

            plugin.ActivatePlugin();
            return plugin;
        }

        public ITemplate CreateTemplate(IReleaseNoteConfiguration wrapper)
        {
            var providerName = wrapper.GetTemplateConfig<BasePluginConfig>().Provider;

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ApplicationException("Template provider name is not specified");
            }

            var plugin = _dependencyResolver.ResolveAll<ITemplate>().GetPlugins(providerName);

            if (plugin == null)
            {
                throw new ApplicationException($"No template plugin found with name {providerName}");
            }

            plugin.ActivatePlugin();
            return plugin;
        }

        public IReleaseNoteLinker CreateReleaseNoteLinker()
        {
            return new ReleaseNoteLinker();
        }

        public ICommitEnrichment CreateCommitEnrichment(IReleaseNoteConfiguration wrapper)
        {
            var issueTracker = CreateIssueTracker(wrapper);
            return new EnrichCommitWithIssueTracker(issueTracker, wrapper);
        }
    }
}