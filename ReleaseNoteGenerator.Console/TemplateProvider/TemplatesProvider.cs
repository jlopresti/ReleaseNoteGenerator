using System;
using log4net;
using Ninject;
using Ninject.Activation;
using Ranger.Console.Models;
using Ranger.Core.Helpers;
using Ranger.Core.TemplateProvider;

namespace Ranger.Console.TemplateProvider
{
    public class TemplatesProvider : Provider<ITemplate>
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(TemplatesProvider));
        protected override ITemplate CreateInstance(IContext context)
        {
            _logger.Debug("[TMP] Try getting template provider from config");
            var config = context.Kernel.Get<ReleaseNoteConfiguration>();

            Guard.IsValidConfig(() => config.Config);
            Guard.ProviderRequired(() => config.Config.Template);

            var provider = config.Config.Template.GetTypeProvider<ITemplate>();
            if (provider != null)
            {
                Guard.ValidateConfigParameter(provider, () => config.Config.Template);
                _logger.DebugFormat("[TMP] Template provider found : {0}", provider.Name);
                var isc = context.Kernel.TryGet(provider);
                return isc as ITemplate;
            }
            throw new ApplicationException($"No provider found with id : {config.Config.Template.GetProvider()}");
        }
    }
}