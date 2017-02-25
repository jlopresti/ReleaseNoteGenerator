using System.Collections.Generic;
using System.IO;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.Binder;
using Ranger.NetCore.Models.Template;
using Ranger.NetCore.TemplateProvider;

namespace Ranger.NetCore.RazorHtml.TemplateProvider
{
    [Provider("htmlFile", ConfigurationType = typeof(HtmlFileTemplateConfig))]
    [ConfigurationParameterValidation("file")]
    public class HtmlFileTemplate : ITemplate
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlFileTemplate));
        private HtmlFileTemplateConfig _config;
        private RazorEngineWrapper _razor;

        public HtmlFileTemplate(HtmlFileTemplateConfig config)
        {
            _config = config;
            _razor = new RazorEngineWrapper();
            Guard.IsNotNull(() => _config);
        }

        public string Build(string releaseNumber, List<ReleaseNoteEntry> entries)
        {
            Guard.IsValidFilePath(() => _config.File);
            
            return _razor.Run(File.ReadAllText(_config.File), new ReleaseNoteViewModel { Tickets = entries, Release = releaseNumber });
        }
    }
}