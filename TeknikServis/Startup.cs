using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeknikServis.Startup))]
namespace TeknikServis
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
