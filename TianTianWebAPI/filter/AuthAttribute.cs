using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TianTianWebAPI.filter
{
    public class AuthAttribute : FilterAttribute, IAuthorizationFilter
    {
        public string permission { get; set; }


        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                 || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
            {
                return;
            }
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // 403 we know who you are, but you haven't been granted access
                if (!((System.Security.Claims.ClaimsIdentity)filterContext.HttpContext.User.Identity).HasClaim(x => x.Type.Equals("permission") && x.Value.Equals(permission)))
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