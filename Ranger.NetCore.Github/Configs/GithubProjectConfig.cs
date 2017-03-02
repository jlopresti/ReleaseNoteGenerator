using System.ComponentModel.DataAnnotations;

namespace Ranger.NetCore.Github.Configs
{
    public class GithubProjectConfig
    {
        [Required]
        public string Host { get; set; }
        public string Login { get; set; }
        public string Apikey { get; set; }
        [Required]
        public string Project { get; set; }
        [Required]
        public string Owner { get; set; }
        [Required]
        public string ProdBranch { get; set; }
        [Required]
        public string ReleaseBranchPattern { get; set; }
        [Required]
        public string MessageCommitPattern { get; set; }

        public void Setup(GithubConfig config)
        {
            Host = Host ?? config.Host;
            Login = Login ?? config.Login;
            Apikey = Apikey ?? config.Apikey;
            Project = Project ?? config.Project;
            Owner = Owner ?? config.Owner;
            ProdBranch = ProdBranch ?? config.ProdBranch;
            ReleaseBranchPattern = ReleaseBranchPattern ?? config.ReleaseBranchPattern;
            MessageCommitPattern = MessageCommitPattern ?? config.MessageCommitPattern;
        }
    }
}