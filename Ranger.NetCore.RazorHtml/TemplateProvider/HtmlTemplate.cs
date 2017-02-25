using System.Collections.Generic;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.Binder;
using Ranger.NetCore.Models.Template;
using Ranger.NetCore.TemplateProvider;

namespace Ranger.NetCore.RazorHtml.TemplateProvider
{
    [Provider("html", ConfigurationType = typeof(HtmlTemplateConfig))]
    [ConfigurationParameterValidation("html")]
    public class HtmlTemplate : ITemplate
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