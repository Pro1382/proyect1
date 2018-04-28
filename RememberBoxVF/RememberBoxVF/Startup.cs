using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RememberBoxVF.Startup))]
namespace RememberBoxVF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
