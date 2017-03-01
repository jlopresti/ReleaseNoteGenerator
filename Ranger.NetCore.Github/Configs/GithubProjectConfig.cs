namespace Ranger.NetCore.Github.Configs
{
    public class GithubProjectConfig
    {
        public string Host { get; set; }
        public string Login { get; set; }
        public string Apikey { get; set; }
        public string Project { get; set; }
        public string Owner { get; set; }
        public string ProdBranch { get; set; }
        public string ReleaseBranchPattern { get; set; }
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