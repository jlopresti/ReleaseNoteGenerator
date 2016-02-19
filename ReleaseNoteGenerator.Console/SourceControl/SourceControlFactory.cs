using System;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.SourceControl
{
    public class GithubSourceControlFactory : ISourceControlFactory
    {
        public ISourceControlProvider GetProvider(Config settings)
        {
            switch (settings.SourceControlType)
            {
                case Common.SourceControl.Github:
                    return new GithubSourceControl(settings.SourceControl);                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}