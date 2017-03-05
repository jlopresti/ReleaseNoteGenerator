namespace Ranger.NetCore
{
    public interface ISourceControlPluginConfiguration : IPluginConfiguration
    {
        string MessageCommitPattern { get; set; }
        string ExcludeCommitPattern { get; set; }
    }
}