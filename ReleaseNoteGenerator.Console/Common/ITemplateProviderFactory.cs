using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    internal interface ITemplateProviderFactory
    {
        ITemplateProvider GetProvider(Config settings);
    }
}