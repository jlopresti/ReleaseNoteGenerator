using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.TemplateProvider
{
    internal interface ITemplateProviderFactory
    {
        ITemplateProvider GetProvider(Config settings);
    }
}