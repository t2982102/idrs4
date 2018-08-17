using idrs4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace idrs4.Services.UserInteraction
{
    public class UserStoreService : IUserStoreService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserStoreService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {

            _userManager = userManager;
            _roleManager = roleManager;
        }


        /// <summary>
        /// 根据用户获取Claim信息 liyouming
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IList<Claim>> GetUserClaimsByUser(ApplicationUser user)
        {
            return await _userManager.GetClaimsAsync(user);

        }
        public async Task<IList<Claim>> GetRoleClaimsByRole(ApplicationRole role)
        {
            return await _roleManager.GetClaimsAsync(role);

        }
        public async Task<IList<Claim>> GetAllClaimsByUser(ApplicationUser user)
        {
            var allclaims = new List<Claim>();
            var userclaims = await _userManager.GetClaimsAsync(user);
            allclaims.AddRange(userclaims);

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var roleval in roles)
            {
                var roleinfo = await _roleManager.FindByNameAsync(roleval);
                var roleclaims = await _roleManager.GetClaimsAsync(roleinfo);
                allclaims.AddRange(roleclaims);
            }
            return allclaims;
        }

        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> ValidatorUser(ApplicationUser user, string password)
        {

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<ApplicationUser> FindByName(string username)
        {

            return await _userManager.FindByNameAsync(username);
        }

        public async Task<ApplicationUser> FindByUserId(string userid)
        {
            return await _userManager.FindByIdAsync(userid);
        }

        public async Task<ApplicationUser> FindByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public IQueryable<ApplicationUser> Users()
        {
            var reslist = _userManager.Users;
            return reslist;
        }
    }
}
