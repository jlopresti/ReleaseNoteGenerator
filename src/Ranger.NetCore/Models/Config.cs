using Newtonsoft.Json.Linq;

namespace Ranger.NetCore.Models
{
    public class Config
    {
        public JObject SourceControl { get; set; }
        public JObject IssueTracker { get; set; }
        public JObject Template { get; set; }
        public JObject Publish { get; set; }
    }
}