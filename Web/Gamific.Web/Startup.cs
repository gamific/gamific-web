using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Vlast.Gamific.Web.Startup))]

namespace Vlast.Gamific.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
