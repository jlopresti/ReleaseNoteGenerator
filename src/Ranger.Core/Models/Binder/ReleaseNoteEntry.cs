using System.Collections.Generic;

namespace Ranger.Core.Models.Binder
{
    public class ReleaseNoteEntry
    {
        public ReleaseNoteEntry()
        {
            Authors = new List<string>();
            AdditionalData = new Dictionary<string, object>();
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string CommitUrl { get; set; }
        public string IssueUrl { get; set; }
        public Status Status { get; set; }
        public IDictionary<string, object> AdditionalData { get; set; }
        public string Project { get; set; }
    }
}