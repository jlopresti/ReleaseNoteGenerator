using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models;

namespace Ranger.NetCore.Publisher
{
    public abstract class BasePublisherPlugin<T> : IPublisherPlugin
        where T : IPluginConfiguration
    {
        public IReleaseNoteConfiguration ConfigurationManager { get; }
        public T Configuration { get; private set; }

        protected BasePublisherPlugin(IReleaseNoteConfiguration configuration)
        {
            ConfigurationManager = configuration;
        }

        public void Activate()
        {
            Configuration = ConfigurationManager.GetPublisherConfig<T>();
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

        public abstract bool Publish(string releaseNumber, string output);

        public abstract string PluginId { get; }
    }
}