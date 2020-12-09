using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class AdminSeedAccount : UserAccount
    {
        public string Password { get; set; }

        public UserAccount ToUserAccount()
        {
            this.EmailConfirmed = true;
            this.UserName = this.Email;
            return this;
        }
    }
}
