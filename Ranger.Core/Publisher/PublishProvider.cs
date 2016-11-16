using System;
using log4net;
using Ninject;
using Ninject.Activation;
using Ranger.Core.Helpers;
using Ranger.Core.Models;

namespace Ranger.Core.Publisher
{
    public class PublishProvider : Provider<IPublisher>
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(PublishProvider));
        protected override IPublisher CreateInstance(IContext context)
        {
            _logger.Debug("[PBS] Try getting publisher provider from config");

            var config = context.Kernel.Get<IReleaseNoteConfiguration>();

            Guard.IsValidConfig(() => config.Config);
            Guard.ProviderRequired(() => config.Config.Publish);

            var provider = config.Config.Publish.GetTypeProvider<IPublisher>();
            if (provider != null)
            {
                Guard.ValidateConfigParameter(provider, () => config.Config.Publish);
                _logger.DebugFormat("[PBS] Publisher provider found : {0}", provider.Name);
                var isc = context.Kernel.TryGet(provider);
                return isc as IPublisher;
            }
            throw new ApplicationException($"No provider found with id : {config.Config.Publish.GetProvider()}");
        }
    }
}