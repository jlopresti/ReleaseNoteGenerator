using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json.Linq;
using Ranger.Core.Common;
using Ranger.Core.Helpers;
using Ranger.Core.Models;
using Ranger.Core.Models.Binder;
using Ranger.Core.Models.Template;

namespace Ranger.Core.TemplateProvider
{
    [Provider("htmlFile")]
    [ConfigurationParameterValidation("file")]
    public class HtmlFileTemplate : ITemplate
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlFileTemplate));
        private HtmlFileTemplateConfig _config;
        private RazorEngineWrapper _razor;

        public HtmlFileTemplate(JObject templateConfigPath)
        {
            _config = templateConfigPath.ToObject<HtmlFileTemplateConfig>();
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