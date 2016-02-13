using System.Threading.Tasks;

namespace ReleaseNoteGenerator.Console
{
    internal interface IConsoleApplication
    {
        Task Run(string[] args);
    }
}