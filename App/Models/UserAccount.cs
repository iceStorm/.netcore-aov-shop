using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Models
{
    public class UserAccount : IdentityUser
    {
        public override string UserName => this.Email;


        [Required(ErrorMessage = "Vui lòng nhập Họ")]
        public string SurName { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Tên lót và Tên")]
        public string FirstName { get; set; }


        public string AvatarUrl { get; set; }
        [BindProperty]
        [NotMapped]
        public string FilePond { get; set; }



        [BindNever]
        public ICollection<GameAccount> BoughtAccounts { get; set; }

        
        /// <summary>
        /// Paste the values from $this to $source.
        /// </summary>
        /// <param name="source">The variable that want to earn the values from.</param>
        public void PasteValues(UserAccount source)
        {
            source.SurName = this.SurName;
            source.FirstName = this.FirstName;

            if (this.FilePond != null)
            {
                source.AvatarUrl = Regex.Replace(this.FilePond.Split("\"")[1], @"\\\\+", @"/");
            }
        }
    }
}
