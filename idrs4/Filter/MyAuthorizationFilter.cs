using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Filter
{
    //public class MyAuthorizationFilter : AuthorizeFilter
    public class MyAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public string permission { get; set; }


        public void OnAuthorization(AuthorizationFilterContext context)
        {


            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            if (!(context.ActionDescriptor is ControllerActionDescriptor))
            {
                return;
            }
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var claims = context.HttpContext.User.Claims;
                
               var permissions = claims.Where(c => c.Type.Equals("permission")&c.Value.Equals(permission));
                if (permissions.Count() > 0)
                {
                    return;
                }
                else
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new RedirectResult("/Account/Login");
            }

        }
    }
}
