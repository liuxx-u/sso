using sso.Authentication;
using System;
using System.Web;
using System.Web.Mvc;

namespace sso.client2.Controllers
{
    public class AccountController : Controller
    {
        public static SsoAuthenticationOptions SsoAuthenticationOptions { get; }

        static AccountController()
        {
            SsoAuthenticationOptions = new SsoAuthenticationOptions
            {
                //直接跳转至server登陆
                LoginPath = "http://localhost:58806/Account/Login"
            };
        }
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        public ActionResult LoginNotify(string token)
        {
            string cookieKey = SsoAuthenticationOptions.CookieName;
            HttpContext.Response.SetCookie(new HttpCookie(cookieKey, token));
            return Content("");
        }

        public ActionResult LogoutNotify()
        {
            var responseCookie = HttpContext.Response.Cookies[SsoAuthenticationOptions.CookieName];
            if (responseCookie != null)
                responseCookie.Expires = DateTime.Now.AddMinutes(-1);

            return Content("");
        }
    }
}