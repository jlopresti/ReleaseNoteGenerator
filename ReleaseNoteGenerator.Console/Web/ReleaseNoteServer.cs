using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Ninject;
using Ninject.Web.WebApi;
using Ranger.Console.Common;

namespace Ranger.Console.Web
{
    public class ReleaseNoteServer : IDisposable
    {
        private const string HttpLocalhost = "http://localhost:9042";
        private IDisposable _server;
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteServer));
        public IDisposable Start()
        {
            NinjectKernel.Instance.Load(new WebApiModule());
            _server = Microsoft.Owin.Hosting.WebApp.Start<Startup>(HttpLocalhost);
            _logger.Info($"[APP] Server available at : {HttpLocalhost}");
            System.Diagnostics.Process.Start(HttpLocalhost);
            return _server;
        }

        public void Dispose()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
        }
    }
}
