using System.Collections.Generic;
using log4net;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Models.Binder;
using ReleaseNoteGenerator.Console.Models.Template;

namespace ReleaseNoteGenerator.Console.TemplateProvider
{
    [Provider("html")]
    [ConfigurationParameterValidation("html")]
    class HtmlTemplateProvider : ITemplateProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlTemplateProvider));
        private HtmlTemplateConfig _config;
        private RazorEngineWrapper _razor;

        public HtmlTemplateProvider(JObject templateConfigPath)
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