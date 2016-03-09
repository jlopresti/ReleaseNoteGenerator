using System.Collections.Generic;
using ReleaseNoteGenerator.Console.Models.Binder;

namespace ReleaseNoteGenerator.Console.TemplateProvider
{
    public interface ITemplate
    {
        string Build(string releaseNumber, List<ReleaseNoteEntry> releaseNoteModel);
    }
}