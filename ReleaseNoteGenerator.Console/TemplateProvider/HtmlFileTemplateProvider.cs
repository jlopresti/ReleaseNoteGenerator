using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using ReleaseNoteGenerator.Console.Models.Binder;
using ReleaseNoteGenerator.Console.Models.Template;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.TemplateProvider
{
    [Provider("htmlFile")]
    public class HtmlFileTemplateProvider : ITemplateProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlFileTemplateProvider));
        private HtmlFileTemplateConfig _config;

        public HtmlFileTemplateProvider(JObject templateConfigPath)
        {
            _config = templateConfigPath.ToObject<HtmlFileTemplateConfig>();
            if (_config == null)
            {
                _logger.Error("Invalid jira config", new JsonException("Json is invalid"));
                return;
            }
        }

        public string Build(List<ReleaseNoteEntry> entries)
        {
            if (_config.HtmlFile == null) return string.Empty;
            return Engine.Razor.RunCompile(File.ReadAllText(_config.HtmlFile), "releasenote", null, new { Tickets = entries });
        }
    }
}