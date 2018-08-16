using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TianTianWebAPI.filter
{
    public class AuthAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // 403 we know who you are, but you haven't been granted access
                if (!filterContext.HttpContext.User.IsInRole(Roles))
                {

                    filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                }
            }
            else
            {
                // 401 who are you? go login and then try again
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}