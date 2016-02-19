using System;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    public class TemplateProviderFactory : ITemplateProviderFactory
    {
        public ITemplateProvider GetProvider(Config settings)
        {
            switch (settings.TemplateType)
            {
                case Template.Html:
                    return new HtmlTemplateProvider(settings.Template);
                case Template.File:
                    return new HtmlFileTemplateProvider(settings.Template);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}