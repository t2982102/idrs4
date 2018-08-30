using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace idrs4.PoJo
{
    public static class ExPrincipal
    {

        public static bool IsPermission(ClaimsPrincipal user, string perimission)
        {
            var permissions = user.FindAll("permission");
            return permissions.Where(c => c.Value.Equals("local."+perimission)).Count()>0;
        }
    }
}
