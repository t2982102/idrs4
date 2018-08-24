using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace idrs4.Controllers
{
    public class CommonJsonController : Controller
    {
        private ConfigurationDbContext _db;
        public CommonJsonController(ConfigurationDbContext db)
        {
            _db = db;
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
    }
}