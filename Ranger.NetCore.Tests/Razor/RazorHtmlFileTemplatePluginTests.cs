using System;
using System.Collections.Generic;
using System.IO;
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
    public class RazorHtmlFileTemplatePluginTests
    {
        [Fact]
        public void Should_throw_an_application_exception_when_file_field_is_missing_from_config()
        {
            var rnc = Substitute.For<IReleaseNoteConfiguration>();
            rnc.GetTemplateConfig<RazorHtmlFileTemplateConfig>().Returns(_ => new RazorHtmlFileTemplateConfig());
            var plugin = new RazorHtmlFileTemplatePlugin(rnc);
            Action action = () => plugin.Activate();
            action.ShouldThrow<ApplicationException>();
        }

        [Fact]
        public void Should_throw_an_application_exception_when_file_does_not_exists()
        {
            var rnc = Substitute.For<IReleaseNoteConfiguration>();
            rnc.GetTemplateConfig<RazorHtmlFileTemplateConfig>().Returns(_ => new RazorHtmlFileTemplateConfig() { File = "toto.html" });
            var plugin = new RazorHtmlFileTemplatePlugin(rnc);
            Action action = () => plugin.Activate();
            action.ShouldThrow<ApplicationException>();
        }


        [Fact]
        public void Should_Return_Html_Bind_To_ModelWith_Template()
        {
            var rnc = Substitute.For<IReleaseNoteConfiguration>();
            rnc.GetTemplateConfig<RazorHtmlFileTemplateConfig>()
                .Returns(_ => new RazorHtmlFileTemplateConfig() { File = Path.Combine(AppContext.BaseDirectory, "template.html") });
            var plugin = new RazorHtmlFileTemplatePlugin(rnc);
            plugin.Activate();
            var result = plugin.Build("1.0.0", new List<ReleaseNoteEntry>() { new ReleaseNoteEntry() });
            result.Should().Be("Release 1.0.0 with 1 Tickets");
        }
    }
}
