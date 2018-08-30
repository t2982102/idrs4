using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using idrs4.Models;
using idrs4.Models.AccountViewModels;
using idrs4.Models.bootstrapTableList;
using idrs4.PoJo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace idrs4.Controllers
{
    [Authorize]
    public class RoleManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleMangeer;
        private readonly ILogger _logger;

        public RoleManageController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleMangeer = roleManager;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterRole(RegisterRoleViewModel model)
        {
            Result rs = new Result();
            if (string.IsNullOrEmpty(model.RoleName))
            {

                rs.ErrorCode = -20;
                rs.ErrorMessage = "角色名不能为空！";
                return Json(rs);
            }
            if (string.IsNullOrEmpty(model.ClientName))
            {
                rs.ErrorCode = -20;
                rs.ErrorMessage = "客户端标识不能为空！";
                return Json(rs);
            }
            if (string.IsNullOrEmpty(model.Description))
            {
                rs.ErrorCode = -20;
                rs.ErrorMessage = "角色描述不能为空！";
                return Json(rs);
            }


            if (await _roleMangeer.RoleExistsAsync(model.RoleName))
            {
                rs.ErrorCode = -21;
                rs.ErrorMessage = "该角色名已存在无法创建！";
                return Json(rs);
            }
            model.RoleName = model.RoleName.Replace(" ", "");
            model.Description = model.Description.Replace(" ", "");
            model.ClientName = model.ClientName.Replace(" ", "");
            var role = new ApplicationRole { Name = model.RoleName, Description = model.Description, ClientName = model.ClientName };
            var result = await _roleMangeer.CreateAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new role.");
                rs = Result.PassResult();
            }
            else
            {
                rs = Result.ErrorResult(-20);
                rs.ErrorMessage = result.Errors.FirstOrDefault().Description;
            }
            return Json(rs);
        }

        [HttpPost]
        public IActionResult AllRoleList(int offset, int limit, string order,string search,string clientname)
        {
            RoleList rlist = new RoleList();
            var allroles = _roleMangeer.Roles;
            if (!string.IsNullOrEmpty(search))
            {
                allroles = allroles.Where(c => c.Name.Contains(search));
            }
            if (!string.IsNullOrEmpty(clientname))
            {
                allroles = allroles.Where(c => c.ClientName.Equals(clientname));
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
        public async Task<IActionResult> DeleteRole(string id)
        {
            Result rs = new Result();
            if (string.IsNullOrEmpty(id))
            {
                rs = Result.ErrorResult(-2);
                return Json(rs);
            }
            var role =await _roleMangeer.FindByIdAsync(id);
            if (role == null)
            {
                rs = Result.ErrorResult(-3);
                return Json(rs);
            }
            bool alldel = true;
            foreach (var claim in await _roleMangeer.GetClaimsAsync(role))
            {
              var removers=await _roleMangeer.RemoveClaimAsync(role, claim);
                alldel = alldel && removers.Succeeded;
            }
            if (alldel)
            {
                var delrs = await _roleMangeer.DeleteAsync(role);
                if (delrs.Succeeded)
                {
                    rs = Result.PassResult();
                }
                else
                {
                    rs = Result.ErrorResult(-25);
                    rs.ErrorMessage = "删除角色失败！";
                    return Json(rs);
                }
            }
            else
            {
                rs = Result.ErrorResult(-25);
                rs.ErrorMessage = "删除角色失败！";
                return Json(rs);
            }
            return Json(rs);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}