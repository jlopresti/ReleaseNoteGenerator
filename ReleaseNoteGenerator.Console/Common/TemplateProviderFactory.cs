using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    public class TemplateProviderFactory : ITemplateProviderFactory
    {
        public ITemplateProvider GetProvider(Settings settings)
        {
            return new HtmlTemplateProvider(settings.TemplateConfig);
        }
    }
}