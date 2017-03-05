using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.SourceControl;

namespace Ranger.NetCore.SourceControl
{
    public abstract class BaseSourceControlPlugin<T> : ISourceControl
        where T : ISourceControlPluginConfiguration
    {
        public IReleaseNoteConfiguration ConfigurationManager { get; }
        public T Configuration { get; private set; }

        protected BaseSourceControlPlugin(IReleaseNoteConfiguration configuration)
        {
            ConfigurationManager = configuration;
        }

        public void ActivatePlugin()
        {
            Configuration = ConfigurationManager.GetSourceControlConfig<T>();
            ValidateConfig(Configuration);
            OnPluginActivated();
        }

        protected virtual void OnPluginActivated()
        {

        }

        private void ValidateConfig(T config)
        {
            var ctx = new ValidationContext(config);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(config, ctx, results, true);
            if (!isValid)
            {
                throw new ApplicationException(results.Aggregate($"[{typeof(T).Name}] ", (x, y) => x + y.ErrorMessage + Environment.NewLine));
            }
        }

        public abstract Task<List<CommitInfo>> GetCommits(string releaseNumber);

        public abstract Task<List<CommitInfo>> GetCommitsFromPastRelease(string release);
        public abstract string PluginId { get; }
    }
}