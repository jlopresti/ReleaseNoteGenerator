using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RazorEngine;
using RazorEngine.Templating;

namespace ReleaseNoteGenerator.Console.Common
{
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
            return Engine.Razor.RunCompile(File.ReadAllText(_config.Html), "releasenote", null, new { Tickets = entries });
        }
    }
}