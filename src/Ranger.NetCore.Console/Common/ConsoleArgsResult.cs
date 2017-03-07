namespace Ranger.NetCore.Console.Common
{
    public class ConsoleArgsResult<T>
    {
        public ConsoleArgsResult(T option, bool success)
        {
            Success = success;
            Parameters = new SettingsWrapper<T>(option);
        }

        public bool Success { get;private set; }
        public SettingsWrapper<T> Parameters { get;private set; }
    }
}