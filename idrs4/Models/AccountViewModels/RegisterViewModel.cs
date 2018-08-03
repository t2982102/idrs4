using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "密码{0}至少{2}最大不能超过{1}", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "确认密码与密码不匹配")]
        public string ConfirmPassword { get; set; }
    }
}
