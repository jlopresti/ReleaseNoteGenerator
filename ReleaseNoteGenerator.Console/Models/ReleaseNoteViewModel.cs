using System.Collections.Generic;
using Ranger.Console.Models.Binder;

namespace Ranger.Console.Models
{
    public class ReleaseNoteViewModel
    {
        public List<ReleaseNoteEntry> Tickets { get; set; }
        public string Release { get; set; }
    }
}