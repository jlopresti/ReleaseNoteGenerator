using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ranger.Web.Startup))]
namespace Ranger.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
