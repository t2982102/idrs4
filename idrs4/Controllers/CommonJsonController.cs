using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using idrs4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace idrs4.Controllers
{
    public class CommonJsonController : Controller
    {
        private  ConfigurationDbContext _db;
        private readonly RoleManager<ApplicationRole> _roleMangeer;
        public CommonJsonController(ConfigurationDbContext db, RoleManager<ApplicationRole> roleManager)
        {
            _db = db;
            _roleMangeer = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetClientName(string ClientId=null)
        {
            var Clients = _db.Clients.Where(c => c.Enabled.Equals(true)).Select(c=>new {Id=c.Id,Name=c.ClientId, Description=c.ClientName });
            if (!string.IsNullOrEmpty(ClientId))
            {
                Clients = Clients.Where(c => c.Name.Equals(ClientId));
            }
            return Json(Clients.ToList());
            
        }
        [HttpPost]
        public IActionResult GetRoleName(string RoleName = null)
        {
            var roles = _roleMangeer.Roles.Select(c => new { Id = c.Id, roleName = c.Name });
            if (!string.IsNullOrEmpty(RoleName))
            {
                roles = roles.Where(c => c.roleName.Equals(RoleName));
            }
            //var Clients = _db.cl.Where(c => c.Enabled.Equals(true)).Select(c => new { Id = c.Id, Name = c.ClientId, Description = c.ClientName });
            //if (!string.IsNullOrEmpty(ClientId))
            //{
            //    Clients = Clients.Where(c => c.Name.Equals(ClientId));
            //}
            return Json(roles.ToList());

        }
    }
}