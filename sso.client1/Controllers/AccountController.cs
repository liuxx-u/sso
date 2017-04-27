using System;
using sso.Authentication;
using System.Web;
using System.Web.Mvc;

namespace sso.client1.Controllers
{
    public class AccountController : Controller
    {
        public static SsoAuthenticationOptions SsoAuthenticationOptions { get; }

        static AccountController()
        {
            SsoAuthenticationOptions = new SsoAuthenticationOptions
            {
                //使用本地登陆页
                LoginPath = "/Account/Login"
            };
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/";
            }
            ViewBag.ReturnUrl = returnUrl;
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