using System.Collections.Generic;
using Ranger.NetCore.Models.Binder;

namespace Ranger.NetCore.TemplateProvider
{
    public interface ITemplate : IRangerPlugin
    {
        string Build(string releaseNumber, List<ReleaseNoteEntry> releaseNoteModel);
    }
}