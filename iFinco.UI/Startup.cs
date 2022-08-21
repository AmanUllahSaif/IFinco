using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iFinco.UI.Startup))]
namespace iFinco.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
