using System;
using System.Threading.Tasks;

namespace Ranger.NetCore.Console.Common
{
    internal interface IConsoleApplication<T> where T : IConsoleApplicationConfiguration
    {
        Task<int> Run(T args);
    }
}