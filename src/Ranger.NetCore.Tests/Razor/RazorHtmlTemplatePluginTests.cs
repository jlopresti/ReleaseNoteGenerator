using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.Binder;
using Ranger.NetCore.RazorHtml;
using Ranger.NetCore.RazorHtml.Configs;
using Ranger.NetCore.RazorHtml.TemplateProvider;
using Xunit;

namespace Ranger.NetCore.Tests.Razor
{
    public class RazorHtmlTemplatePluginTests
    {
        [Fact]
        public void Should_throw_an_application_exception_when_html_field_is_missing_from_config()
        {
            var rnc = Substitute.For<IReleaseNoteConfiguration>();
            rnc.GetTemplateConfig<RazorHtmlTemplateConfig>().Returns(_ => new RazorHtmlTemplateConfig());
            var plugin = new RazorHtmlTemplatePlugin(rnc);
            Action action = () => plugin.Activate();
            action.ShouldThrow<ApplicationException>();
        }

        [Fact]
        public void Should_Return_Html_Bind_To_Model()
        {
            var rnc = Substitute.For<IReleaseNoteConfiguration>();
            rnc.GetTemplateConfig<RazorHtmlTemplateConfig>().Returns(_ => new RazorHtmlTemplateConfig() { Html = "Release @Model.Release with @Model.Tickets.Count Tickets" });
            var plugin = new RazorHtmlTemplatePlugin(rnc);
            plugin.Activate();
            var result = plugin.Build("1.0.0", new List<ReleaseNoteEntry>() { new ReleaseNoteEntry() });
            result.Should().Be("Release 1.0.0 with 1 Tickets");
        }
    }
}
