
namespace Ranger.NetCore.Models.IssueTracker
{
    public class JiraConfiguration
    {
        public string Host { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Project { get; set; }
    }
}