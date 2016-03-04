using System.Collections.Generic;

namespace ReleaseNoteGenerator.Console.Models.Binder
{
    public class ReleaseNoteEntry
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string CommitUrl { get; set; }
        public string IssueUrl { get; set; }
        public Status Status { get; set; }
    }
}