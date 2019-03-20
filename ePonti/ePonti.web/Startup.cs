using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ePonti.web.Startup))]
namespace ePonti.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
