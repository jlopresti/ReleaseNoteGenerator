using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;
using RazorEngine.Templating;

namespace ReleaseNoteGenerator.Console.Common
{
    public interface ITemplateProvider
    {
        string Build(List<ReleaseNoteEntry> releaseNoteModel);
    }

    class HtmlTemplateProvider : ITemplateProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlTemplateProvider));
        private TemplateConfig _config;

        public HtmlTemplateProvider(string templateConfigPath)
        {
            if (string.IsNullOrEmpty(templateConfigPath))
            {
                _logger.Error("Jira config must be provided", new NullReferenceException("configPath"));
                return;
            }
            if (!File.Exists(templateConfigPath))
            {
                _logger.Error("Jira config not found, please check config path", new FileNotFoundException($"{templateConfigPath} doesn't exist."));
                return;
            }
            _config = File.ReadAllText(templateConfigPath).ToObject<TemplateConfig>();
            if (_config == null)
            {
                _logger.Error("Invalid jira config", new JsonException("Json is invalid"));
                return;
            }
        }

        public string Build(List<ReleaseNoteEntry> entries)
        {
            if (!File.Exists(_config.Template))
            {
                _logger.Error("Jira config not found, please check config path", new FileNotFoundException($"{_config.Template} doesn't exist."));
                return string.Empty;
            }
            return Engine.Razor.RunCompile(File.ReadAllText(_config.Template), "releasenote", null, new { Tickets = entries });
        }
    }
}