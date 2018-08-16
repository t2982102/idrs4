using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TianTianWebAPI.Controllers
{
    
    public class HomeController : Controller
    {
        
        [Authorize]
        public ActionResult Index()
        {
            //ViewBag.Title = "Home Page";

            return Json(from c in HttpContext.GetOwinContext().Authentication.User.Claims select new { c.Type, c.Value }, JsonRequestBehavior.AllowGet);
            //return View();
        }
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        //}
    }
}
