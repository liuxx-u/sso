using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using sso.Authentication;
using sso.server.ViewModels;
using sso.Utils;

namespace sso.server.Controllers
{
    public class AccountController : Controller
    {
        public static SsoAuthenticationOptions SsoAuthenticationOptions { get; }

        static AccountController()
        {
            SsoAuthenticationOptions = new SsoAuthenticationOptions
            {
                UserClientStore=new SampleUserClientStore(),
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


        /// <summary>
        /// 服务端登陆执行方法
        /// 其他站需要jsonp Get调用
        /// </summary>
        /// <param name="signInfo">登陆信息</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<JavaScriptResult> SignIn(SignInfo signInfo)
        {
            if (signInfo.ReturnUrl.IsNullOrWhiteSpace())
            {
                signInfo.ReturnUrl = Request.ApplicationPath;
            }
            //登陆验证，生成ClaimsIdentity
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, signInfo.UserName),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims);

            var code = await SsoAuthenticationOptions.GetLoginJavascriptCode(identity, signInfo.ReturnUrl);
            return JavaScript(code);
        }

        /// <summary>
        /// 注销
        /// 其他站需要jsonp Get调用
        /// </summary>
        /// <returns></returns>
        public JavaScriptResult SignOut()
        {
            if (User == null || User.Identity == null) return JavaScript("");

            var code = SsoAuthenticationOptions.GetLogoutJavascriptCode(User.Identity.Name);
            return JavaScript(code);
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