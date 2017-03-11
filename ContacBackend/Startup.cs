using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContacBackend.Startup))]
namespace ContacBackend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
