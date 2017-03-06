using System.Collections.Generic;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models.Binder;

namespace Ranger.NetCore.Template
{
    public interface ITemplatePlugin : IRangerPlugin
    {
        string Build(string releaseNumber, List<ReleaseNoteEntry> releaseNoteModel);
    }
}