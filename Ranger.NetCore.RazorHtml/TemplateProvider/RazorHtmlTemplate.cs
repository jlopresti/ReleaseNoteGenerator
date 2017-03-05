using System.Collections.Generic;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.Binder;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.RazorHtml.Configs;
using Ranger.NetCore.TemplateProvider;

namespace Ranger.NetCore.RazorHtml.TemplateProvider
{
    public class RazorHtmlTemplate : BaseTemplatePlugin<RazorHtmlTemplateConfig>
    {
        public override string PluginId => "razor-html";

        private readonly RazorEngineWrapper _razor;

        public RazorHtmlTemplate(IReleaseNoteConfiguration configuration)
            : base(configuration)
        {
            _razor = new RazorEngineWrapper();
        }

        public override string Build(string releaseNumber, List<ReleaseNoteEntry> entries)
        {
            return _razor.Run(Configuration.Html, new ReleaseNoteViewModel { Tickets = entries, Release = releaseNumber });
        }
    }
}