using System.Collections.Generic;
using System.IO;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.Binder;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.RazorHtml.Configs;
using Ranger.NetCore.Template;

namespace Ranger.NetCore.RazorHtml.TemplateProvider
{
    public class RazorHtmlFileTemplatePlugin : BaseTemplatePlugin<RazorHtmlFileTemplateConfig>
    {
        public override string PluginId => "razor-file";

        private readonly RazorEngineWrapper _razor;

        public RazorHtmlFileTemplatePlugin(IReleaseNoteConfiguration configuration)
            : base(configuration)
        {
            _razor = new RazorEngineWrapper();
        }

        public override string Build(string releaseNumber, List<ReleaseNoteEntry> entries)
        {        
            return _razor.Run(File.ReadAllText(Configuration.File), new ReleaseNoteViewModel { Tickets = entries, Release = releaseNumber });
        }
    }
}