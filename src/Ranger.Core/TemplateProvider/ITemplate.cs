using System.Collections.Generic;
using Ranger.Core.Models.Binder;

namespace Ranger.Core.TemplateProvider
{
    public interface ITemplate
    {
        string Build(string releaseNumber, List<ReleaseNoteEntry> releaseNoteModel);
    }
}