using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Models.AccountViewModels
{
    public class LoginInputModel
    {
        [Required(ErrorMessage ="用户名不能为空！")]
        
        public string Username { get; set; }
        [Required(ErrorMessage ="密码不能为空！")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
