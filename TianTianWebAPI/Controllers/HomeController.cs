using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TianTianWebAPI.filter;

namespace TianTianWebAPI.Controllers
{
    
    public class HomeController : Controller
    {
        
        [AuthAttribute(permission ="mvc4.view")]
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
