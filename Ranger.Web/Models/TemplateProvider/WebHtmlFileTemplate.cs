using System.Collections.Generic;
using System.IO;
using System.Web;
using Ranger.Core.Common;
using Ranger.Core.Helpers;
using Ranger.Core.Models;
using Ranger.Core.Models.Binder;
using Ranger.Core.Models.Template;
using Ranger.Core.TemplateProvider;

namespace Ranger.Web.Models.TemplateProvider
{
    [Provider("webFile", ConfigurationType = typeof(HtmlFileTemplateConfig))]
    [ConfigurationParameterValidation("file")]
    public class WebHtmlFileTemplate : ITemplate
    {
        private HtmlFileTemplateConfig _config;
        private RazorEngineWrapper _razor;

        public WebHtmlFileTemplate(HtmlFileTemplateConfig config)
        {
            _config = config;
            _razor = new RazorEngineWrapper();
            Guard.IsNotNull(() => _config);
        }

        public string Build(string releaseNumber, List<ReleaseNoteEntry> entries)
        {
            Guard.IsNotNull(() => _config.File);
            var templatePath =  HttpContext.Current.Server.MapPath("~/App_Data/templates");
            var filePath = Path.Combine(templatePath, _config.File);
            Guard.IsValidFilePath(() => filePath);
            return _razor.Run(File.ReadAllText(filePath), new ReleaseNoteViewModel { Tickets = entries, Release = releaseNumber });
        }
    }
}