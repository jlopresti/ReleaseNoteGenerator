using log4net;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.Template;

namespace Ranger.NetCore.Common
{
    public class ProviderFactory : IProviderFactory
    {
        private readonly IDependencyResolver _dependencyResolver;
        private readonly ILog _logger;

        public ProviderFactory(IDependencyResolver dependencyResolver, ILog logger)
        {
            _dependencyResolver = dependencyResolver;
            _logger = logger;
        }
        public ISourceControlPlugin CreateSourceControl(IReleaseNoteConfiguration wrapper)
        {
            var providerName = wrapper.GetSourceControlConfig<BasePluginConfig>().Provider;

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ApplicationException("Source control provider name is not specified");
            }

            var plugin = _dependencyResolver.ResolveAll<ISourceControlPlugin>().GetPlugins(providerName);

            if (plugin == null)
            {
                throw new ApplicationException($"No source control plugin found with name {providerName}");
            }

            plugin.Activate();
            return plugin;
        }

        public IIssueTrackerPlugin CreateIssueTracker(IReleaseNoteConfiguration wrapper)
        {
            var providerName = wrapper.GetIssueTrackerConfig<BasePluginConfig>().Provider;

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ApplicationException("Issue tracker provider name is not specified");
            }

            var plugin = _dependencyResolver.ResolveAll<IIssueTrackerPlugin>().GetPlugins(providerName);

            if (plugin == null)
            {
                throw new ApplicationException($"No issue tracker plugin found with name {providerName}");
            }

            plugin.Activate();
            return plugin;
        }

        public IPublisherPlugin CreatePublisher(IReleaseNoteConfiguration wrapper)
        {
            var providerName = wrapper.GetPublisherConfig<BasePluginConfig>().Provider;

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ApplicationException("Publisher provider name is not specified");
            }

            var plugin = _dependencyResolver.ResolveAll<IPublisherPlugin>().GetPlugins(providerName);

            if (plugin == null)
            {
                throw new ApplicationException($"No publisher plugin found with name {providerName}");
            }

            plugin.Activate();
            return plugin;
        }

        public ITemplatePlugin CreateTemplate(IReleaseNoteConfiguration wrapper)
        {
            var providerName = wrapper.GetTemplateConfig<BasePluginConfig>().Provider;

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ApplicationException("Template provider name is not specified");
            }

            var plugin = _dependencyResolver.ResolveAll<ITemplatePlugin>().GetPlugins(providerName);

            if (plugin == null)
            {
                throw new ApplicationException($"No template plugin found with name {providerName}");
            }

            plugin.Activate();
            return plugin;
        }
    }
}