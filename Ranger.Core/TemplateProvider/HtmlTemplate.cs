using System.Collections.Generic;
using log4net;
using Newtonsoft.Json.Linq;
using Ranger.Core.Common;
using Ranger.Core.Helpers;
using Ranger.Core.Models;
using Ranger.Core.Models.Binder;
using Ranger.Core.Models.Template;

namespace Ranger.Core.TemplateProvider
{
    [Provider("html", ConfigurationType = typeof(HtmlTemplateConfig))]
    [ConfigurationParameterValidation("html")]
    class HtmlTemplate : ITemplate
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlTemplate));
        private HtmlTemplateConfig _config;
        private RazorEngineWrapper _razor;

        public HtmlTemplate(HtmlTemplateConfig config)
        {
            _config = config;
            _razor = new RazorEngineWrapper();
            Guard.IsNotNull(() => _config);
        }

        public string Build(string releaseNumber, List<ReleaseNoteEntry> entries)
        {
            if (_config.Html == null) return string.Empty;
            return _razor.Run(_config.Html, new ReleaseNoteViewModel { Tickets = entries, Release = releaseNumber });
        }
    }
}