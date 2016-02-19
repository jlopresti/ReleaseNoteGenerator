using System;
using Newtonsoft.Json.Linq;

namespace ReleaseNoteGenerator.Console.Common
{
    internal class Config
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
                var result = Enum.TryParse(SourceControl.Value<string>("provider"), out enu);
                return enu;
            }
        }

        public IssueTracker IssueTrackerType { get; set; }
        public Template TemplateType { get; set; }
        public Publish PublishType { get; set; }
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
        Html, Unknown
    }
    public enum Publish
    {
        Local, Unknown
    }
}