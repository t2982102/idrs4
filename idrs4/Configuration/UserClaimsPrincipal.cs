using idrs4.Models;
using idrs4.Services.UserInteraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace idrs4.Configuration
{
    public class UserClaimsPrincipal : IUserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly IUserStoreService _storeService;
        public RoleManager<ApplicationRole> RoleManager { get; private set; }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public IdentityOptions Options { get; private set; }
        public UserClaimsPrincipal(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> optionsAccessor, IUserStoreService storeService)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }
            if (optionsAccessor == null || optionsAccessor.Value == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager));
            }
            UserManager = userManager;
            Options = optionsAccessor.Value;
            RoleManager = roleManager;
            _storeService = storeService;
        }

        public async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            //var claims = await _storeService.GetAllClaimsByUser(user);
            //ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            //return await Task.FromResult(claimsPrincipal);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var id = await GenerateClaimsAsync(user);
            return new ClaimsPrincipal(id);

        }

        protected virtual async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            var userName = await UserManager.GetUserNameAsync(user);
            var id = new ClaimsIdentity("Identity.Application", // REVIEW: Used to match Application scheme
                Options.ClaimsIdentity.UserNameClaimType,
                Options.ClaimsIdentity.RoleClaimType);
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName));
            if (UserManager.SupportsUserSecurityStamp)
            {
                id.AddClaim(new Claim(Options.ClaimsIdentity.SecurityStampClaimType,
                    await UserManager.GetSecurityStampAsync(user)));
            }
            if (UserManager.SupportsUserClaim)
            {
                id.AddClaims(await UserManager.GetClaimsAsync(user));
            }

            var roles = await UserManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                    var role = await RoleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        if (role.ClientName.Equals("local"))
                        {
                            id.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, roleName));
                            id.AddClaims(await RoleManager.GetClaimsAsync(role));
                        }
                    }
            }
            return id;
        }

    }
}
