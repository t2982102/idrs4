using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TianTianWebAPI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            
            //Request.GetOwinContext().Authentication.SignOut();
            Request.GetOwinContext().Authentication.SignOut("oidc");
            Request.GetOwinContext().Authentication.SignOut("Cookies");
            //return View();
            return Redirect("/");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogoutByServer(string sid)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var sessionId = claimsPrincipal?.FindFirst("sid")?.Value;
            if (sessionId != null && sessionId == sid)
            {
                Request.GetOwinContext().Authentication.SignOut("Cookies");
            }

            return Content(this.Request.Url?.DnsSafeHost + "退出成功。", "text/html");
        }

    }
}