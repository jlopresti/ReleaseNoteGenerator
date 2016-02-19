using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    internal interface ITemplateProviderFactory
    {
        ITemplateProvider GetProvider(Settings settings);
    }
}