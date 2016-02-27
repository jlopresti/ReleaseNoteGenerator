using System.Collections.Generic;
using ReleaseNoteGenerator.Console.Models.Binder;

namespace ReleaseNoteGenerator.Console.Models
{
    public class ReleaseNoteViewModel
    {
        public List<ReleaseNoteEntry> Tickets { get; set; }
    }
}