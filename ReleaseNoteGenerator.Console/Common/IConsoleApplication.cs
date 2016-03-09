using System.Threading.Tasks;

namespace Ranger.Console.Common
{
    internal interface IConsoleApplication
    {
        Task<int> Run(string[] args);
    }
}