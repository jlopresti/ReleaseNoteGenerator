using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    public class TemplateProviderFactory : ITemplateProviderFactory
    {
        public ITemplateProvider GetProvider(JObject settings)
        {
            return new HtmlTemplateProvider(settings.ConfigPath);
        }
    }
}