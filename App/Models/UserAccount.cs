using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class UserAccount : IdentityUser
    {

        [Required(ErrorMessage = "Vui lòng nhập Họ")]
        public string SurName { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Tên lót và Tên")]
        public string FirstName { get; set; }


        public string AvatarUrl { get; set; }


        [BindNever]
        public ICollection<GameAccount> BoughtAccounts { get; set; }


        public int TotalMoney { get; set; } = 0;   //  Số tiền trong Tài khoản ảo

    }
}
