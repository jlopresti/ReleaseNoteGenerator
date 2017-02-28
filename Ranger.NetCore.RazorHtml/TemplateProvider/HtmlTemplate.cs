using System.Collections.Generic;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.Binder;
using Ranger.NetCore.Models.Template;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.TemplateProvider;

namespace Ranger.NetCore.RazorHtml.TemplateProvider
{
    [Provider("html", ConfigurationType = typeof(HtmlTemplateConfig))]
    [ConfigurationParameterValidation("html")]
    public class HtmlTemplate : BaseTemplatePlugin<HtmlTemplateConfig>
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlTemplate));
        private RazorEngineWrapper _razor;

        public HtmlTemplate(IReleaseNoteConfiguration configuration)
            : base(configuration)
        {
            _razor = new RazorEngineWrapper();
        }

        public override string Build(string releaseNumber, List<ReleaseNoteEntry> entries)
        {
            if (Configuration.Html == null) return string.Empty;
            return _razor.Run(Configuration.Html, new ReleaseNoteViewModel { Tickets = entries, Release = releaseNumber });
        }
    }
}