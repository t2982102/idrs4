using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Models.PermissionManageViewModels
{
    public class DelViewModel
    {
        public string roleName { get; set; }
        public string permissionValue { get; set; }
    }
    public class DelViewModels
    {
        public List<DelViewModel> ids { get; set; }
    }
}
