using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Models.AccountViewModels
{
    public class RegisterRoleViewModel
    {
        [Required(ErrorMessage ="角色名称不能为空！")]
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "角色描述不能为空！")]
        [Display(Name = "角色描述")]
        public string Description { get; set; }


        [Required(ErrorMessage = "客户端标识不能为空！")]
        [Display(Name ="客户端标识")]
        public string ClientName { get; set; }
    }
}
