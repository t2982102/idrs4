using idrs4.Models;
using idrs4.Models.bootstrapTableList;
using idrs4.Models.PermissionManageViewModels;
using idrs4.PoJo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace idrs4.Controllers
{
    public class PermissionManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleMangeer;
        private readonly ILogger _logger;
        public PermissionManageController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleMangeer = roleManager;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> AllPermissionList(int offset, int limit, string order,string search)
        {

            PermissionList plist = new PermissionList();
            //List<Claim> claims =new List<Claim>();
            List<ApplicationPermission> aplist = new List<ApplicationPermission>();

            var allroles = _roleMangeer.Roles;

            foreach (var role in allroles)
            {

                foreach (var claims in await _roleMangeer.GetClaimsAsync(role))
                {
                    ApplicationPermission ap = new ApplicationPermission();
                    ap.RoleName = role.Name;
                    ap.PermissionType = claims.Type;
                    ap.PermissionValue = claims.Value;
                    aplist.Add(ap);
                }

                //claims.AddRange(_roleMangeer.GetClaimsAsync(role).Result.ToList());

            }
            if (!string.IsNullOrEmpty(search))
            {
                aplist = aplist.Where(c => c.PermissionValue.Contains(search)).ToList();
            }
            plist.total = aplist.Count();
            if (limit != 0)
            {
                if (order.Equals("desc"))
                {

                    aplist = aplist.OrderByDescending(c => c.PermissionType).Skip(offset).Take(limit).ToList();
                }
                else
                {
                    aplist = aplist.OrderBy(c => c.PermissionType).Skip(offset).Take(limit).ToList();
                }
            }
            plist.rows = aplist;
            return Json(plist);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePermission(DelViewModels models)
        {
            //var rolest = 
            
            Result rs = new Result();
            if (models.ids.Count()== 0)
            {
                rs = Result.ErrorResult(-2);
                return Json(rs);
            }
            else
            {
                bool removeok = true;
                foreach (var model in models.ids)
                {
                  var role =await _roleMangeer.FindByNameAsync(model.roleName);
                    var claims = await _roleMangeer.GetClaimsAsync(role);
                    var claim= claims.Where(c => c.Value.Equals(model.permissionValue)).FirstOrDefault();
                    var rmovers = await _roleMangeer.RemoveClaimAsync(role, claim);
                    if (rmovers.Succeeded)
                    {
                        removeok = removeok & true;
                    }
                    else
                    {
                        removeok = removeok & false;
                    }
                }
                if (removeok)
                {
                    rs = Result.PassResult();

                }
                else
                {
                    rs = Result.ErrorResult(-25);
                    rs.ErrorMessage = "删除失败！";
                }
                return Json(rs);

            }
        }
        public async Task<IActionResult> RegisterPermission(string roleName, string permissionValue)
        {
            Result rs = new Result();
            if (string.IsNullOrEmpty(roleName) || string.IsNullOrEmpty(permissionValue) )
            {
                rs = Result.ErrorResult(-2);
                return Json(rs);
            }
            //var user = _userManager.GetUserAsync(User);
            var role=await _roleMangeer.FindByIdAsync(roleName);
            Claim addclaim = new Claim("permission",role.ClientName+"."+permissionValue);

            var addrs = await _roleMangeer.AddClaimAsync(role,addclaim);
            if (addrs.Succeeded)
            {
                rs = Result.PassResult();
            }
            else
            {
                rs.ErrorCode = -104;
                rs.ErrorMessage = addrs.Errors.FirstOrDefault().Description;
            }
            return Json(rs);
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}