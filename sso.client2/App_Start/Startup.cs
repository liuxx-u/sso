using Microsoft.Owin;
using Owin;
using sso.Authentication;
using sso.client2;
using sso.client2.Controllers;

[assembly: OwinStartup(typeof(Startup))]

namespace sso.client2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSsoCookieAuthentication(AccountController.SsoAuthenticationOptions);
        }
    }
}
