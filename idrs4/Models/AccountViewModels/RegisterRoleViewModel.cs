using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Models.AccountViewModels
{
    public class RegisterRoleViewModel
    {
        [Required]
        [Display(Name = "角色名称")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "角色标识")]
        public string NormalizedName { get; set; }

        
    }
}
