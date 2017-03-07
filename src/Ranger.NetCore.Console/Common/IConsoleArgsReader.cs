namespace Ranger.NetCore.Console.Common
{
    internal interface IConsoleArgsReader
    {
        ConsoleArgsResult<T> ReadConsoleArgs<T>(string[] args);
    }
}