using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Ranger.Console.Common;

[assembly: OwinStartup(typeof(Ranger.Console.Web.Startup))]

namespace Ranger.Console.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            var fileSystem = new PhysicalFileSystem(@"./todomvc");

            var options = new FileServerOptions
            {
                EnableDirectoryBrowsing = true,
                FileSystem = fileSystem
            };
            app.UseFileServer(options);
            app.UseNinjectMiddleware(() => NinjectKernel.Instance).UseNinjectWebApi(config);
            app.UseWelcomePage("/");
        }
    }
}