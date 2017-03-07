using System;
using log4net;
using Ninject;
using Ninject.Activation;
using Ranger.Core.Helpers;
using Ranger.Core.Models;

namespace Ranger.Core.IssueTracker
{
    public class IssueTrackerProvider : Provider<IIssueTracker>
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(IssueTrackerProvider));
        protected override IIssueTracker CreateInstance(IContext context)
        {
            _logger.Debug("[IT] Try getting issue tracker provider from config");
            var config = context.Kernel.Get<IReleaseNoteConfiguration>();
            var provider = config.Config.IssueTracker.GetTypeProvider<IIssueTracker>();

            Guard.IsValidConfig(() => config.Config);
            Guard.ProviderRequired(() => config.Config.IssueTracker);

            if (provider != null)
            {
                Guard.ValidateConfigParameter(provider, () => config.Config.IssueTracker);
                var isc = context.Kernel.TryGet(provider);
                _logger.DebugFormat("[IT] Issue tracker provider found : {0}", provider.Name);
                return isc as IIssueTracker;
            }
            throw new ApplicationException($"No provider found with id : {config.Config.IssueTracker.GetProvider()}");
        }
    }
}