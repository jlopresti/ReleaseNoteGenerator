using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ranger.NetCore.Models.SourceControl
{
    public class CommitInfo : IReleaseNoteKey
    {
        public CommitInfo()
        {
            Id = Guid.NewGuid().ToString();
            Authors = new List<string>();
        }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public bool HasExtractedKey { get; set; }
        public IDictionary<string, object> AdditionalData { get; set; }
        public string Project { get; set; }

        public void ExtractKeyFromTitle(string pattern)
        {
            var keyExtractor =  new Regex(pattern, RegexOptions.IgnoreCase);
            var match = keyExtractor.Match(Title);
            if (match.Success)
            {
                Id = match.Value;
                HasExtractedKey = true;
            }
        }
    }
}