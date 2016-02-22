namespace ReleaseNoteGenerator.Console.Models.IssueTracker
{
    public class Issue : IReleaseNoteKey
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
    }
}