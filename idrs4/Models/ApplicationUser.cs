using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using idrs4.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace idrs4.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
    }
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        public string ClientName { get; set; }
    }

    public class ApplicationPermission
    {
        public string PermissionType { get; set; }
        public string PermissionValue { get; set; }

        public string RoleName { get; set; }
    }

   
}
