using System;
using System.IO;
using Microsoft.Extensions.DependencyModel;
using Ranger.NetCore.Models;
using RazorLight;
using RazorLight.Extensions;

namespace Ranger.NetCore.RazorHtml
{
    public class RazorEngineWrapper
    {
        private IRazorLightEngine _service;

        public RazorEngineWrapper()
        {
            string path = Path.Combine(System.AppContext.BaseDirectory, "views");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            _service = EngineFactory.CreatePhysical(path);
        }

        public string Run(string template, ReleaseNoteViewModel vm)
        {
            return _service.ParseString(template, vm);
        }
    }
}