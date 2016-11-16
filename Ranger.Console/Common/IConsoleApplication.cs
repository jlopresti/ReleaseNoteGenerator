using System;
using System.Threading.Tasks;

namespace Ranger.Console.Common
{
    internal interface IConsoleApplication : IDisposable
    {
        Task<int> Run(string[] args);
    }
}