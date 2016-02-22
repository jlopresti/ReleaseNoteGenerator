using System.Collections.Generic;
using ReleaseNoteGenerator.Console.Models.Binder;

namespace ReleaseNoteGenerator.Console.TemplateProvider
{
    public interface ITemplateProvider
    {
        string Build(List<ReleaseNoteEntry> releaseNoteModel);
    }
}