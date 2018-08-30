using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using idrs4.Models.bootstrapTableList;
using idrs4.PoJo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace idrs4.Controllers
{
    public class ClientManageController : Controller
    {
        private readonly ConfigurationDbContext _db;

        public ClientManageController(ConfigurationDbContext db)
        {
            _db = db;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[MyAuthorizationFilter(permission = "UserManage.List")]
        public IActionResult AllClientList(int offset, int limit, string order, string search)
        {
            ClientManageList clmlist = new ClientManageList();
            
            var allclient= _db.Clients.Include(o => o.ClientSecrets).Include(o=>o.AllowedScopes).Include(o=>o.IdentityProviderRestrictions).Include(o=>o.Claims).Include(o=>o.AllowedCorsOrigins).Include(o=>o.AllowedGrantTypes).Include(o=>o.RedirectUris).Include(o=>o.PostLogoutRedirectUris).Include(o=>o.Properties).Where(c => c.Id > 0);
            //var allroles = _userManager.Users;



            if (!string.IsNullOrEmpty(search))
            {
                allclient = allclient.Where(c => c.ClientId.Contains(search));
            }
            //allclient = allclient.Include(o => o.ClientSecrets);
            clmlist.total = allclient.Count();
            if (limit != 0)
            {
                if (order.Equals("desc"))
                {
                    allclient = allclient.OrderByDescending(c => c.Id).Skip(offset).Take(limit);
                }
                else
                {
                    allclient = allclient.OrderBy(c => c.Id).Skip(offset).Take(limit);
                }
            }
            clmlist.rows = ClientTablePoJo.GetClientTablePoJoList(allclient.ToList());
            return Json(clmlist);
        }
    }
}