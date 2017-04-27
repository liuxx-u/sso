using Microsoft.Owin;
using Owin;
using sso.Authentication;
using sso.server;
using sso.server.Controllers;

[assembly: OwinStartup(typeof(Startup))]

namespace sso.server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSsoCookieAuthentication(AccountController.SsoAuthenticationOptions);
        }
    }
}
