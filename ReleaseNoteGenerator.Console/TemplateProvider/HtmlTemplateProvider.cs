    using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RazorEngine;
using RazorEngine.Templating;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models.Binder;
using ReleaseNoteGenerator.Console.Models.Template;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.TemplateProvider
{
    [Provider("html")]
    class HtmlTemplateProvider : ITemplateProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlTemplateProvider));
        private HtmlTemplateConfig _config;

        public HtmlTemplateProvider(JObject templateConfigPath)
        {
            _config = templateConfigPath.ToObject<HtmlTemplateConfig>();
            if (_config == null)
            {
                _logger.Error("Invalid jira config", new JsonException("Json is invalid"));
                return;
            }
        }

        public string Build(List<ReleaseNoteEntry> entries)
        {
            if (_config.Html == null) return string.Empty;
            return Engine.Razor.RunCompile(_config.Html, "releasenote", null, new { Tickets = entries });
        }
    }
}