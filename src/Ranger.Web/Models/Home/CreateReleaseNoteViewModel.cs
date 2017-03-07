using System.Collections.Generic;

namespace Ranger.Web.Models.Home
{
    public class CreateReleaseNoteViewModel
    {
        public List<string> Configs { get; set; }
        public string Release { get; set; }
        public string Config { get; set; }
        public IEnumerable<string> Teams { get; set; }
        public string Team { get; set; }
        public List<Compo> Components { get; set; }
    }

    public class Compo
    {
        public string Label { get; set; }
        public bool IsChecked { get; set; }

    }
}