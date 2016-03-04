using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json.Linq;
using RazorEngine;
using RazorEngine.Templating;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models;
using ReleaseNoteGenerator.Console.Models.Binder;
using ReleaseNoteGenerator.Console.Models.Template;

namespace ReleaseNoteGenerator.Console.TemplateProvider
{
    [Provider("htmlFile")]
    [ConfigurationParameterValidation("file")]
    public class HtmlFileTemplateProvider : ITemplateProvider
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(HtmlFileTemplateProvider));
        private HtmlFileTemplateConfig _config;
        private RazorEngineWrapper _razor;

        public HtmlFileTemplateProvider(JObject templateConfigPath)
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