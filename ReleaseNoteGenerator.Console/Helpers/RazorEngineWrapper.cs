using Ranger.Console.Models;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Ranger.Console.Helpers
{
    public class RazorEngineWrapper
    {
        private IRazorEngineService _service;

        public RazorEngineWrapper()
        {
            var tfc = new TemplateServiceConfiguration();
            tfc.DisableTempFileLocking = true;
            tfc.CachingProvider = new DefaultCachingProvider(_ => { });

            _service = RazorEngineService.Create(tfc);
        }

        public string Run(string template, ReleaseNoteViewModel vm)
        {
            return _service.RunCompile(template, "releasenote", typeof(ReleaseNoteViewModel), vm);
        }
    }
}