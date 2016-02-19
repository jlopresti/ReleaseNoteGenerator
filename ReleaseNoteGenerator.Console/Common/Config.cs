using System;
using Newtonsoft.Json.Linq;

namespace ReleaseNoteGenerator.Console.Common
{
    public class Config
    {
        public JObject SourceControl { get; set; }
        public JObject IssueTracker { get; set; }
        public JObject Template { get; set; }
        public JObject Publish { get; set; }

        public SourceControl SourceControlType
        {
            get
            {

                Common.SourceControl enu;
                var result = Enum.TryParse(SourceControl.Value<string>("provider"),true, out enu);
                return enu;
            }
        }

        public IssueTracker IssueTrackerType
        {
            get
            {

                Common.IssueTracker enu;
                var result = Enum.TryParse(IssueTracker.Value<string>("provider"), true, out enu);
                return enu;
            }
        }

        public Template TemplateType
        {
            get
            {

                Common.Template enu;
                var result = Enum.TryParse(Template.Value<string>("provider"), true, out enu);
                return enu;
            }
        }

        public Publish PublishType
        {
            get
            {

                Common.Publish enu;
                var result = Enum.TryParse(Publish.Value<string>("provider"), true, out enu);
                return enu;
            }
        }

    }

    public enum SourceControl
    {
        Github, Unknown
    }
    public enum IssueTracker
    {
        Jira, Unknown
    }
    public enum Template
    {
        Html, File, Unknown
    }
    public enum Publish
    {
        Local, Unknown
    }
}