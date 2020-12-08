using App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class SignUpViewModel : LoginViewModel
    {
        public UserAccount User { get; set; }


        [Required (ErrorMessage = "Vui lòng nhập Mật khẩu xác nhận")]
        [Compare ("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }

    }
}
