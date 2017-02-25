using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.IssueTracker;
using Ranger.NetCore.Linker;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.SourceControl;
using Ranger.NetCore.Models.Template;
using Ranger.NetCore.Publisher;
using Ranger.NetCore.RazorHtml.TemplateProvider;
using Ranger.NetCore.SourceControl;
using Ranger.NetCore.TemplateProvider;

namespace Ranger.NetCore.Console
{
    class ProviderFactory : IProviderFactory
    {
        private readonly IEnumerable<ISourceControl> _sourceControls;
        private readonly IEnumerable<IIssueTracker> _issueTrackers;
        private readonly IEnumerable<ITemplate> _templates;
        private readonly IEnumerable<IPublisher> _publishers;

        public ProviderFactory(IEnumerable<ISourceControl> sourceControls, 
            IEnumerable<IIssueTracker> issueTrackers,
            IEnumerable<ITemplate> templates,
            IEnumerable<IPublisher> publishers)
        {
            _sourceControls = sourceControls;
            _issueTrackers = issueTrackers;
            _templates = templates;
            _publishers = publishers;
        }
        public ISourceControl CreateSourceControl(IReleaseNoteConfiguration wrapper)
        {
            var t = _sourceControls.SingleOrDefault(
                x => x.GetType().GetTypeInfo()
                    .GetCustomAttribute<ProviderAttribute>()
                    .Name.Equals(wrapper.Config.SourceControl.GetProvider(),
                        StringComparison.CurrentCultureIgnoreCase));
            return new StubSourceControl();
        }

        public IIssueTracker CreateIssueTracker(IReleaseNoteConfiguration wrapper)
        {
            return new StubIssueTracker();
        }

        public IPublisher CreatePublisher(IReleaseNoteConfiguration wrapper)
        {
            return new NoOpPublisher();
        }

        public ITemplate CreateTemplate(IReleaseNoteConfiguration wrapper)
        {
            return new HtmlFileTemplate(wrapper.Config.Template.ToObject<HtmlFileTemplateConfig>());
        }

        public IReleaseNoteLinker CreateReleaseNoteLinker()
        {
            return new ReleaseNoteLinker();
        }
    }
}