using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProyectoOwin.Startup))]
namespace ProyectoOwin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}