using Microsoft.Owin;
using Owin;
using sso.Authentication;
using sso.client1;
using sso.client1.Controllers;

[assembly: OwinStartup(typeof(Startup))]

namespace sso.client1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSsoCookieAuthentication(AccountController.SsoAuthenticationOptions);
        }
    }
}
