using ReleaseNoteGenerator.Console.Common;

namespace ReleaseNoteGenerator.Console.Models
{
    public class Issue : IReleaseNoteKey
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
    }
}