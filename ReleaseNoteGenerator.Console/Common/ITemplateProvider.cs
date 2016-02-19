using System;
using System.Collections.Generic;
using RazorEngine.Templating;

namespace ReleaseNoteGenerator.Console.Common
{
    public interface ITemplateProvider
    {
        string Build(List<ReleaseNoteEntry> releaseNoteModel);
    }
}