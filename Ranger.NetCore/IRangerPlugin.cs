namespace Ranger.NetCore
{
    public interface IRangerPlugin
    {
        string PluginId { get; }
        void ActivatePlugin();

    }
}