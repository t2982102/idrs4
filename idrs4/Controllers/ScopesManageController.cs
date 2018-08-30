using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace idrs4.Controllers
{
    public class ScopesManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}