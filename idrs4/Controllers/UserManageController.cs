using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using idrs4.Filter;
using idrs4.Models;
using idrs4.Models.AccountViewModels;
using idrs4.Models.bootstrapTableList;
using idrs4.Models.ManageViewModels;
using idrs4.PoJo;
using idrs4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace idrs4.Controllers
{
    public class UserManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleMangeer;
        private readonly IEmailSender _emailSender;
        private readonly ISMSSender _smsSender;
        private readonly ILogger _logger;
        public UserManageController(UserManager<ApplicationUser> userManager,
          RoleManager<ApplicationRole> roleManager, ILogger<AccountController> logger, IEmailSender emailSender,ISMSSender smsSender)
        {
            _userManager = userManager;
            _roleMangeer = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }
        [HttpPost]
        [MyAuthorizationFilter(permission = "UserManage.Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            Result rs = new Result();
            if (ModelState.IsValid)
            {
               
                if (string.IsNullOrEmpty(model.Email))
                {

                    rs.ErrorCode = -20;
                    rs.ErrorMessage = "用户名不能为空！";
                    return Json(rs);
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    rs.ErrorCode = -20;
                    rs.ErrorMessage = "密码不能为空！";
                    return Json(rs);
                }
                if (string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    rs.ErrorCode = -20;
                    rs.ErrorMessage = "确认密码不能为空！";
                    return Json(rs);
                }

                if (!model.ConfirmPassword.Equals(model.Password))
                {
                    rs.ErrorCode = -20;
                    rs.ErrorMessage = "两次输入密码不相同！";
                    return Json(rs);
                }


                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // 这里是一注册就发送邮件给邮箱 可用可不用
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);


                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    rs = Result.PassResult();
                    //return RedirectToLocal(returnUrl);
                }
                else
                {
                    rs = Result.ErrorResult(-6);
                    //rs.ErrorMessage = result.Errors.FirstOrDefault().Description;
                }
            }
            else
            {
                rs = Result.ErrorResult(-21);
            }
            
            //AddErrors(result);

            // If we got this far, something failed, redisplay form
            return Json(rs);
        }


        [HttpPost]
        [MyAuthorizationFilter(permission = "UserManage.List")]
        public IActionResult AllUserList(int offset, int limit, string order,string search)
        {
            UserList rlist = new UserList();
            var allroles = _userManager.Users;
            if (!string.IsNullOrEmpty(search))
            {
                allroles = allroles.Where(c => c.UserName.Contains(search));
            }
            rlist.total = allroles.Count();
            if (limit != 0)
            {
                if (order.Equals("desc"))
                {
                    allroles = allroles.OrderByDescending(c => c.Id).Skip(offset).Take(limit);
                }
                else
                {
                    allroles = allroles.OrderBy(c => c.Id).Skip(offset).Take(limit);
                }
            }
            rlist.rows = allroles.ToList();
            return Json(rlist);
        }


        [HttpPost]
        public async Task<IActionResult> GetRoleFromAndTo(string userid)
        {
            ReslutRolesFromAndTo rrfat = new ReslutRolesFromAndTo();
            Result rs = new Result();
            if (string.IsNullOrEmpty(userid))
            {
                rs = Result.ErrorResult(-2);
                rrfat.result = rs;
                return Json(rrfat);
            }
            var user= await _userManager.FindByIdAsync(userid);
            if (user == null)
            {

                rs = Result.ErrorResult(-3);
                rrfat.result = rs;
                return Json(rrfat);
            }
            var to =(List<string>) await  _userManager.GetRolesAsync(user);

            var from =_roleMangeer.Roles.Where(c => !to.Contains(c.Name)).Select(c => c.Name).ToList();
            rrfat.result = Result.PassResult();
            RolesFromAndTo rfat = new RolesFromAndTo();
            rfat.From = from;
            rfat.To = to;
            rrfat.rolesFromAndTo = rfat;
            return Json(rrfat);
        }

        [HttpPost]
        [MyAuthorizationFilter(permission = "UserManage.Del")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            Result rs = new Result();
            if (string.IsNullOrEmpty(id))
            {
                rs = Result.ErrorResult(-20);
                return Json(rs);
            }
            var delrs=await _userManager.DeleteAsync(await _userManager.FindByIdAsync(id));
            if (delrs.Succeeded)
            {
                rs = Result.PassResult();
            }
            else
            {
                rs = Result.ErrorResult(-25);
                rs.ErrorMessage = delrs.Errors.FirstOrDefault().Description;
            }
            return Json(rs);
        }

        [HttpPost]
        public async Task<IActionResult> UserToRole(string userid,string[] RoleName)
        {
            Result rs = new Result();
            var user = await _userManager.FindByIdAsync(userid);
            var roles = await _userManager.GetRolesAsync(user);
            var result1 = await _userManager.RemoveFromRolesAsync(user, roles);
            var rsroles = await _userManager.AddToRolesAsync(user, RoleName);
            if (rsroles.Succeeded)
            {
                rs = Result.PassResult();
            }
            else
            {
                rs = Result.ErrorResult(-1);
                rs.ErrorMessage = rsroles.Errors.FirstOrDefault().Description;
            }
            return Json(rs);
        }
        // 以防万一角色赋值删除这种情况
        [HttpGet]
        public async Task<IActionResult> UserToRoleByGet(string[] RoleName)
        {
            Result rs = new Result();
            //var user = await _userManager.FindByIdAsync(userid);
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            var result1 = await _userManager.RemoveFromRolesAsync(user, roles);
            var rsroles = await _userManager.AddToRolesAsync(user, RoleName);
            if (rsroles.Succeeded)
            {
                rs = Result.PassResult();
            }
            else
            {
                rs = Result.ErrorResult(-1);
                rs.ErrorMessage = rsroles.Errors.FirstOrDefault().Description;
            }
            return Json(rs);
        }

        [HttpPost]
        public async Task<IActionResult> SendSMS(string PhoneNumber)
        {

            Result rs = new Result();
            var user = await _userManager.GetUserAsync(User);
            if (user.PhoneNumberConfirmed)
            {
                rs.ErrorCode = -41;
                rs.ErrorMessage = "手机已验证过,不可重复验证！";
                return Json(rs);
            }
            if (_userManager.Users.Where(c => c.PhoneNumber.Equals(PhoneNumber)&c.PhoneNumberConfirmed).Count() > 0)
            {
                rs.ErrorCode = -109;
                rs.ErrorMessage = "该手机号已被注册,请更换其他手机号";
                return Json(rs);
            }
            user.PhoneNumber = PhoneNumber;
            var upres = _userManager.UpdateAsync(user);
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, PhoneNumber);
            var rssender=await _smsSender.SendSMSAsync(PhoneNumber, "天天易拉罐后台验证码：[" + code+"]");
            return Json(rs);
        }
        public async Task<ActionResult> VerifyPhoneNumber(string PhoneNumber, string Code)
        {
            Result rs = new Result();
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePhoneNumberAsync(user, PhoneNumber, Code);
            if (result.Succeeded)
            {
                rs = Result.PassResult();
            }
            else
            {
                rs.ErrorCode = -109;
                rs.ErrorMessage = "手机验证失败";
            }
            return Json(rs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            Result rs = new Result();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                rs = Result.ErrorResult(-101);
                
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                rs = Result.ErrorResult(-7);
                rs.ErrorMessage = changePasswordResult.Errors.FirstOrDefault().Description;
                //AddErrors(changePasswordResult);
                return Json(rs);
            }

            //await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            //StatusMessage = "Your password has been changed.";
            rs = Result.PassResult();
            return Json(rs);
            //return RedirectToAction(nameof(ChangePassword));
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}