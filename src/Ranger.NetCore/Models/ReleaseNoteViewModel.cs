using System.Collections.Generic;
using Ranger.NetCore.Models.Binder;

namespace Ranger.NetCore.Models
{
    public class ReleaseNoteViewModel
    {
        public List<ReleaseNoteEntry> Tickets { get; set; }
        public string Release { get; set; }
    }
}