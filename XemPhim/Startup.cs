using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XemPhim.Startup))]
namespace XemPhim
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
