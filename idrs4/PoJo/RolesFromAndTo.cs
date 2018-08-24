using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.PoJo
{
    public class RolesFromAndTo
    {
        public List<string> From { get; set; }
        public List<string> To { get; set; }
    }
    public class ReslutRolesFromAndTo
    {
        public Result result { get; set; }
        public RolesFromAndTo rolesFromAndTo { get; set; }
    }
}
