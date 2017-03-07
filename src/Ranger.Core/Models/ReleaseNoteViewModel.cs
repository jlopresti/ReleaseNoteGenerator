using System.Collections.Generic;
using Ranger.Core.Models.Binder;

namespace Ranger.Core.Models
{
    public class ReleaseNoteViewModel
    {
        public List<ReleaseNoteEntry> Tickets { get; set; }
        public string Release { get; set; }
    }
}