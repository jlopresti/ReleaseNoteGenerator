using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.Binder;

namespace Ranger.NetCore.TemplateProvider
{
    public abstract class BaseTemplatePlugin<T> : ITemplate
        where T : IPluginConfiguration
    {
        public IReleaseNoteConfiguration ConfigurationManager { get; }
        public T Configuration { get; private set; }

        protected BaseTemplatePlugin(IReleaseNoteConfiguration configuration)
        {
            ConfigurationManager = configuration;
        }

        public void ActivatePlugin()
        {
            Configuration = ConfigurationManager.GetTemplateConfig<T>();
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
                throw new ApplicationException(results.Aggregate($"[{typeof(T).Name}] ", (x,y) => x + y.ErrorMessage + Environment.NewLine));
            }
        }

        public abstract string Build(string releaseNumber, List<ReleaseNoteEntry> releaseNoteModel);
        public abstract string PluginId { get; }
    }
}