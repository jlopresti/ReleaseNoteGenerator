namespace Ranger.NetCore.Models
{
    public interface IReleaseNoteConfiguration
    {
        string ReleaseNumber { get; }
        T GetIssueTrackerConfig<T>();
        T GetPublisherConfig<T>();
        T GetSourceControlConfig<T>();
        T GetTemplateConfig<T>();
        bool LoadConfigFile(string path, string release);
    }
}