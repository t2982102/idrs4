using idrs4.PoJo;
using System.Collections.Generic;

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

    public class PermissionList
    {

        public List<ApplicationPermission> rows { get; set;}
        public int total { get; set; }
    }

    public class ClientManageList
    {
        public List<ClientTablePoJo> rows { get; set; }
        public int total { get; set; }
    }
}
