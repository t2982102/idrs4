using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using idrs4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Configuration
{
    /// <summary>
    /// 自定义 Resource owner password 验证器
    /// </summary>
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        /// <summary>
        /// 这里为了演示我们还是使用TestUser作为数据源，
        /// 正常使用此处应当传入一个 用户仓储 等可以从
        /// 数据库或其他介质获取我们用户数据的对象
        /// </summary>
       // private readonly TestUserStore _users;
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISystemClock _clock;

        public CustomResourceOwnerPasswordValidator(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ISystemClock clock)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _clock = clock;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var contextuser = _userManager.FindByNameAsync(context.UserName);
            //此处使用context.UserName, context.Password 用户名和密码来与数据库的数据做校验
            if (contextuser != null)
            {
                if (_userManager.CheckPasswordAsync(contextuser.Result, context.Password).Result)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Username Or Password Incorrect");
                }
                else
                {
                    //验证通过返回结果 
                    //subjectId 为用户唯一标识 一般为用户id
                    //authenticationMethod 描述自定义授权类型的认证方法 
                    //authTime 授权时间
                    //claims 需要返回的用户身份信息单元 此处应该根据我们从数据库读取到的用户信息 添加Claims 如果是从数据库中读取角色信息，那么我们应该在此处添加 此处只返回必要的Claim

                    var user = contextuser.Result;
                    //context.Result = new GrantValidationResult(
                    //  user.Id ?? throw new ArgumentException("Subject ID not set", nameof(user.Id)),
                    //  OidcConstants.AuthenticationMethods.Password, _clock.UtcNow.UtcDateTime,
                    //  user.cl);
                }


               

              

            }


            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
            return Task.CompletedTask;
        }
    }
}
