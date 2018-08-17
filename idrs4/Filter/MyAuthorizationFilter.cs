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
            //var attributeList = new List<object>();
            //attributeList.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true));
            //attributeList.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.DeclaringType.GetCustomAttributes(true));
            //var authorizeAttributes = attributeList.OfType<TestAuthorizeAttribute>().ToList();
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var claims = context.HttpContext.User.Claims;
                var permissions = claims.Where(c => c.Type.Equals("permission"));
                if (permissions.Count() > 0)
                {
                    var verifypermission = permissions.Where(c => c.Value.Contains(permission));
                    if (verifypermission.Count() > 0)
                    {
                        return;
                    }
                    else
                    {
                        context.Result= new ForbidResult();
                        //context.HttpContext.Response.StatusCode = 403;
                    }
                }
                else
                {
                    context.Result = new ForbidResult();
                    //context.HttpContext.Response.StatusCode = 403;
                }
            }
            else
            {
                context.Result = new RedirectResult("/Account/Login");
            }

            // 从claims取出用户相关信息，到数据库中取得用户具备的权限码，与当前Controller或Action标识的权限码做比较

            //if (!authorizeAttributes.Any(s => s.Permission.Equals(userPermissions)))
            //{
            //    context.Result = new JsonResult("没有权限");
            //}
            

        }
    }
}
