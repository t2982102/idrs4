using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using idrs4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Configuration
{
    public class CustomProfileService : IProfileService
    {
        /// <summary>
        /// The logger
        /// </summary>
        //protected readonly ILogger Logger;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        /// <summary>
        /// The users
        /// </summary>
        //protected readonly TestUserStore Users;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestUserProfileService"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <param name="logger">The logger.</param>
        public CustomProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
            _roleManager = roleManager;
            //Logger = logger;
        }

        /// <summary>
        /// 只要有关用户的身份信息单元被请求（例如在令牌创建期间或通过用户信息终点），就会调用此方法
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //context.LogProfileRequest(Logger);
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("查无此用户~");
                //context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            //Add more claims like this
            //claims.Add(new System.Security.Claims.Claim("MyProfileID", user.Id));

            
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {

                
                if (context.Client.ClientId == "mvc4")
                {
                    claims.Add(new System.Security.Claims.Claim("role", role));
                    var roleclaim = await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync(role));
                    claims.AddRange(roleclaim);
                }
            }
            //这个方法是向所有client 下发相应的claims
            context.IssuedClaims = claims;
            //context.LogIssuedClaims(Logger);

            //return Task.CompletedTask;
        }

        /// <summary>
        /// 验证用户是否有效 例如：token创建或者验证
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            //Logger.LogDebug("IsActive called from: {caller}", context.Caller);


            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
