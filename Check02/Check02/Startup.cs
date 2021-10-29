using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Check02.Startup))]
namespace Check02
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
