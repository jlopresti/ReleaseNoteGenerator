using System.Collections.Generic;

namespace Ranger.Web.Models
{
    public class CreateReleaseNoteViewModel
    {
        public List<string> Configs { get; set; }
        public string Release { get; set; }
        public string Config { get; set; }
    }
}