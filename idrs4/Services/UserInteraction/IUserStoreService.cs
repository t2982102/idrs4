using idrs4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace idrs4.Services.UserInteraction
{
    /// <summary>
    /// 这里
    /// </summary>
    public interface IUserStoreService
    {
        /// <summary>
        /// 获取用户Claims信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IList<Claim>> GetUserClaimsByUser(ApplicationUser user);
        Task<IList<Claim>> GetRoleClaimsByRole(ApplicationRole role);


        Task<IList<Claim>> GetAllClaimsByUser(ApplicationUser user);

        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ValidatorUser(ApplicationUser user, string password);
        Task<ApplicationUser> FindByName(string username);
        Task<ApplicationUser> FindByUserId(string userid);
        Task<ApplicationUser> FindByEmail(string email);
        IQueryable<ApplicationUser> Users();

    }
}
