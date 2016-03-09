namespace ReleaseNoteGenerator.Console.Common
{
    public interface IConsoleApplicationConfiguration :IVerboseParameter,ISilentParameter
    {
        bool LoadConfig(string[] args);
    }
}