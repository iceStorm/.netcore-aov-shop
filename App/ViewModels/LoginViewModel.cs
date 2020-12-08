using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class LoginViewModel
    {
        public string ReturnUrl { get; set; }


        [Required (ErrorMessage = "Vui lòng nhập địa chỉ E-mail")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        public string Password { get; set; }

    }
}
