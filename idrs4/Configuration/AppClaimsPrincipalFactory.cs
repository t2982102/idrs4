using idrs4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace idrs4.Configuration
{
    // 这个是添加方法 20180817 废除 该方法已经被UserClaimsPrincipal所替代，写的不够灵活
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser,ApplicationRole>
    {
        public AppClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
           IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            //var myprincipal = ((ClaimsIdentity)principal.Identity).Claims;
            //var rolenamelist=await UserManager.GetRolesAsync(user);
            //foreach (var rolename in rolenamelist)
            //{
            //    var role = await RoleManager.FindByNameAsync(rolename);
            //    ((ClaimsIdentity)principal.Identity).AddClaims(new[] { new Claim("ClientId",role.ClientName )});
            //}

            return principal;
        }
    }
}
