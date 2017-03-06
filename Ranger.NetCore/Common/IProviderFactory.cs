using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.Template;

namespace Ranger.NetCore.Common
{
    public interface IProviderFactory
    {
        ISourceControlPlugin CreateSourceControl(IReleaseNoteConfiguration wrapper);
        IIssueTrackerPlugin CreateIssueTracker(IReleaseNoteConfiguration wrapper);
        IPublisherPlugin CreatePublisher(IReleaseNoteConfiguration wrapper);
        ITemplatePlugin CreateTemplate(IReleaseNoteConfiguration wrapper);
    }
}
