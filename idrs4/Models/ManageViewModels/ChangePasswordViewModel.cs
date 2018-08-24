using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required (ErrorMessage ="原始密码不能为空！")]
        [DataType(DataType.Password)]
        [Display(Name = "原始密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage ="新密码不能为空！")]
        [StringLength(100, ErrorMessage = " {0}长度必须为 {2}~{1}位 .", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新的密码")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage ="确认密码不能为空！")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("NewPassword", ErrorMessage = "新密码与确认密码不相符！.")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
