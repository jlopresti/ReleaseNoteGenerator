using System.Collections.Generic;

namespace Ranger.Web.Models.Configurations
{
    public class ViewConfigurationsViewModel
    {
        public IEnumerable<string> Teams { get; set; }
        public string Team { get; set; }
        public string Config { get; set; }
    }
}