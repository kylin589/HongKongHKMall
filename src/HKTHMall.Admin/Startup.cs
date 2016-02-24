using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HKTHMall.Admin.Startup))]
namespace HKTHMall.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
