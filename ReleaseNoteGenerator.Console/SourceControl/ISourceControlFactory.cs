using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    internal interface ISourceControlFactory
    {
        ISourceControlProvider GetProvider(Config provider);
    }
}