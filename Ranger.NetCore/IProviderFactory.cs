using Ranger.NetCore.Enrichment;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Linker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.Reducer;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.TemplateProvider;

namespace Ranger.NetCore
{
    public interface IProviderFactory
    {
        ISourceControl CreateSourceControl(IReleaseNoteConfiguration wrapper);
        IIssueTracker CreateIssueTracker(IReleaseNoteConfiguration wrapper);
        IPublisher CreatePublisher(IReleaseNoteConfiguration wrapper);
        ITemplate CreateTemplate(IReleaseNoteConfiguration wrapper);
    }
}
