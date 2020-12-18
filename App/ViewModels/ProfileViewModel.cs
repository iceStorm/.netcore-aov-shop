using App.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class ProfileViewModel : UserAccount 
    {
        public ProfileViewModel(UserAccount account)
        {
            account.PasteValues(this);
        }

        public IFormFile AvatarFile { get; set; }
    }
}
