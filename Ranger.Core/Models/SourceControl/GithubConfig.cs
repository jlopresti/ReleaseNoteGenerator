using System.Collections.Generic;

namespace Ranger.Core.Models.SourceControl
{
    public class GithubConfig
    {
        public string Host { get; set; }
        public string Login { get; set; }
        public string Apikey { get; set; }
        public string Owner { get; set; }
        public List<string> Projects { get; set; }
        public string ProdBranch { get; set; }
        public string ReleaseBranchPattern { get; set; }
        public string MessageCommitPattern { get; set; }
    }
}