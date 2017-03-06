using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.IssueTracker;

namespace Ranger.NetCore.IssueTracker
{
    public abstract class BaseIssueTrackerPlugin<T> : IIssueTrackerPlugin
        where T : IPluginConfiguration
    {
        public IReleaseNoteConfiguration ConfigurationManager { get; }
        public T Configuration { get; private set; }

        protected BaseIssueTrackerPlugin(IReleaseNoteConfiguration configuration)
        {
            ConfigurationManager = configuration;
        }

        public void Activate()
        {
            Configuration = ConfigurationManager.GetIssueTrackerConfig<T>();
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
                throw new ApplicationException(results.Aggregate(string.Empty, (x, y) => x + y.ErrorMessage + Environment.NewLine));
            }
        }

        public abstract Task<List<Issue>> GetIssues(string release);

        public abstract Task<Issue> GetIssue(string id);
        public abstract string PluginId { get; }
    }
}