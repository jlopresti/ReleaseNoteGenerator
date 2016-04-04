using System.Collections.Generic;

namespace Ranger.Web.Models.Configurations
{
    public class ViewConfigurationsViewModel
    {
        public IEnumerable<string> Teams { get; set; }
        public string SelectedTeam { get; set; }
        public IEnumerable<ConfigViewModel> Configs { get; set; }
    }
}