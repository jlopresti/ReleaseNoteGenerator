using System.ComponentModel.DataAnnotations;

namespace Ranger.Web.Models.Teams
{
    public class CreateTeamViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}