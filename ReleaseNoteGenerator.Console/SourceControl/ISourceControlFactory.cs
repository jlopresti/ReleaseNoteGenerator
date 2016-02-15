using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    internal interface ISourceControlFactory
    {
        ISourceControlProvider GetProvider(Settings provider);
    }
}