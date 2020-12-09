using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private UserManager<UserAccount> userManager;
        private SignInManager<UserAccount> signInManager;

        public HeaderViewComponent(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }



        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (signInManager.IsSignedIn(HttpContext.User))
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                var role = await userManager.GetRolesAsync(currentUser);

                return View(new HeaderViewModel { User = currentUser, Role = role[0] });
            }


            var viewModel = new HeaderViewModel {  };
            return View(viewModel);
        }


    }
}
