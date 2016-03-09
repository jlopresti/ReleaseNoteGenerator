using System.Collections.Generic;
using log4net;
using Newtonsoft.Json.Linq;
using Ranger.Console.Common;
using Ranger.Console.Helpers;
using Ranger.Console.Models;
using Ranger.Console.Models.Binder;
using Ranger.Console.Models.Template;

namespace Ranger.Console.TemplateProvider
{
    [Provider("html")]
    [ConfigurationParameterValidation("html")]
    class HtmlTemplate : ITemplate
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlTemplate));
        private HtmlTemplateConfig _config;
        private RazorEngineWrapper _razor;

        public HtmlTemplate(JObject templateConfigPath)
        {
            _config = templateConfigPath.ToObject<HtmlTemplateConfig>();
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