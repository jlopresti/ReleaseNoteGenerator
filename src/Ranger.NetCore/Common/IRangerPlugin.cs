namespace Ranger.NetCore.Common
{
    public interface IRangerPlugin
    {
        string PluginId { get; }
        void Activate();

    }
}