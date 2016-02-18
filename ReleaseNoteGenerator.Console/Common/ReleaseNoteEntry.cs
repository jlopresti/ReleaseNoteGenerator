namespace ReleaseNoteGenerator.Console.Common
{
    public class ReleaseNoteEntry
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CommitUrl { get; set; }
        public Status Status { get; set; }
    }
}