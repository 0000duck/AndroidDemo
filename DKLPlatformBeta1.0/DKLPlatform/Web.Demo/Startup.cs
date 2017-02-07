using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web.Demo.Startup))]
namespace Web.Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
