using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Models.bootstrapTableList
{
    public class RoleList
    {
        public List<ApplicationRole> rows { get; set; }
        public int total { get; set; }
    }
    public class UserList
    {
        public List<ApplicationUser> rows { get; set; }
        public int total { get; set; }
    }
}
