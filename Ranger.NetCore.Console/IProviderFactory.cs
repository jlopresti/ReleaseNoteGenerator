using System;
using System.Collections.Generic;
using System.Text;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Linker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.IssueTracker;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.TemplateProvider;

namespace Ranger.NetCore.Console
{
    public interface IProviderFactory
    {
        ISourceControl CreateSourceControl(IReleaseNoteConfiguration wrapper);
        IIssueTracker CreateIssueTracker(IReleaseNoteConfiguration wrapper);
        IPublisher CreatePublisher(IReleaseNoteConfiguration wrapper);
        ITemplate CreateTemplate(IReleaseNoteConfiguration wrapper);
        IReleaseNoteLinker CreateReleaseNoteLinker();
    }
}
