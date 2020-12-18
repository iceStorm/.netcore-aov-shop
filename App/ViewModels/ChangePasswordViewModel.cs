using App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class ChangePasswordViewModel : UserAccount
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu cũ")]
        public string OldPassword { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu mới")]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập lại Mật khẩu mới")]
        [Compare (nameof(NewPassword), ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string NewConfirmPassword { get; set; }

    }
}
