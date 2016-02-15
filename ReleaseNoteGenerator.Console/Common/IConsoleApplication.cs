using System.Threading.Tasks;

namespace ReleaseNoteGenerator.Console.Common
{
    internal interface IConsoleApplication
    {
        Task<int> Run(string[] args);
    }
}