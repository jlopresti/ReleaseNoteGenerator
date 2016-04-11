using System.Collections.Generic;

namespace Ranger.Web.Models.Configurations
{
    public class ConfigViewModel
    {
        public string Name { get; set; }
        public string Configuration { get; set; }
    }

    public class Conf
    {
        public object SourceControl { get; set; }
        public object IssueTracker { get; set; }
        public object Template { get; set; }
    }

    public class CreateConfigViewModel
    {
        public List<string> SourceControlProviders { get; set; }
        public List<string> IssueTrackerProviders { get; set; }
        public List<string> TemplateProviders { get; set; }
        public ConfigViewModel Configuration { get; set; }
        public Conf TestConfig { get; set; }
    }
}