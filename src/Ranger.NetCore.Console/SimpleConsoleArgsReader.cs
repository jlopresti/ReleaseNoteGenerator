using CommandLine;
using Ranger.NetCore.Console.Common;

namespace Ranger.NetCore.Console
{
    public class SimpleConsoleArgsReader : IConsoleArgsReader
    {
        public ConsoleArgsResult<T> ReadConsoleArgs<T>(string[] args)
        {
            return Parser.Default.ParseArguments<T>(args)
                .MapResult((option) => new ConsoleArgsResult<T>(option, true),
                    _ => new ConsoleArgsResult<T>(default(T), false));
        }
    }
}