using System.Collections.Generic;

namespace Ranger.Core.Models.IssueTracker
{
    public class Issue : IReleaseNoteKey
    {
        public Issue()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        public string Title { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }

        public IDictionary<string,object> AdditionalData { get;private  set; }
    }
}