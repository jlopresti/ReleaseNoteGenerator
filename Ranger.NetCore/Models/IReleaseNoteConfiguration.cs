namespace Ranger.NetCore.Models
{
    public interface IReleaseNoteConfiguration
    {
        Config Config { get; }
        string ReleaseNumber { get;}
    }
}