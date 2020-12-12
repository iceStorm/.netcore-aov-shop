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

        
        public void PasteValues(UserAccount source)
        {
            source.SurName = this.SurName;
            source.FirstName = this.FirstName;
            source.AvatarUrl = this.AvatarUrl;
        }
    }
}
