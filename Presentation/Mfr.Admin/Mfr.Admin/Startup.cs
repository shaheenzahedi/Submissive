using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mfr.Admin.Startup))]
namespace Mfr.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            ConfigureAuth(app);
        }
    }
}
