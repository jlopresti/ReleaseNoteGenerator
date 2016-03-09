using System.Collections.Generic;
using Ranger.Console.Models.Binder;

namespace Ranger.Console.TemplateProvider
{
    public interface ITemplate
    {
        string Build(string releaseNumber, List<ReleaseNoteEntry> releaseNoteModel);
    }
}