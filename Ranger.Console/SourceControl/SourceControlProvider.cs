using System;
using log4net;
using Ninject;
using Ninject.Activation;
using Ranger.Console.Models;
using Ranger.Core.Helpers;
using Ranger.Core.SourceControl;

namespace Ranger.Console.SourceControl
{
    public class SourceControlProvider : Provider<ISourceControl>
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(SourceControlProvider));
        protected override ISourceControl CreateInstance(IContext context)
        {
            _logger.Debug("[SC] Try getting source control provider from config");
            var config = context.Kernel.Get<ReleaseNoteConfiguration>();
            Guard.IsValidConfig(() => config.Config);
            Guard.ProviderRequired(() => config.Config.SourceControl);

            var provider = config.Config.SourceControl.GetTypeProvider<ISourceControl>();
            if (provider != null)
            {
                Guard.ValidateConfigParameter(provider, () => config.Config.SourceControl);
                _logger.DebugFormat("[SC] Source control provider found : {0}", provider.Name);
                var isc = context.Kernel.TryGet(provider);
                return isc as ISourceControl;
            }
            throw new ApplicationException($"No provider found with id : {config.Config.SourceControl.GetProvider()}");
        }
    }
}