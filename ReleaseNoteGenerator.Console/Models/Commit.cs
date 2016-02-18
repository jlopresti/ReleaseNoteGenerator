using System;
using System.Text.RegularExpressions;
using ReleaseNoteGenerator.Console.Common;

namespace ReleaseNoteGenerator.Console.Models
{
    public class Commit : IReleaseNoteKey
    {
        public Commit()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public bool HasExtractedKey { get; set; }

        public void ExtractKeyFromTitle(string pattern)
        {
            var keyExtractor =  new Regex("sgr-\\w*", RegexOptions.IgnoreCase);
            var match = keyExtractor.Match(Title);
            if (match.Success)
            {
                Id = match.Value;
                HasExtractedKey = true;
            }
        }
    }
}