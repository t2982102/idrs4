using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Models.AccountViewModels
{
    public class LockViewModel
    {
        public string ReturnUrl { get; set; }
        public string Password { get; set; }

        public string errormsg { get; set; }
    }
}
