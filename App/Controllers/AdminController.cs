using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Infrastructures;
using App.Models;
using App.Repositories.Interfaces;
using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Authorize (Roles = Constants.AdminRole)]
    public class AdminController : Controller
    {
        private IGameAccountRepo gameAccountRepo;
        private IUserAccountRepo userAccountRepo;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<UserAccount> userManager;
        private SignInManager<UserAccount> signInManager;

        public AdminController(
            IGameAccountRepo gameAccountRepo, IUserAccountRepo userAccountRepo,
            RoleManager<IdentityRole> roleManager, 
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager)
        {
            this.gameAccountRepo = gameAccountRepo;
            this.userAccountRepo = userAccountRepo;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }



        public async Task<IActionResult> Dashboard()
        {
            return View(new DashboardViewModel { GameAccounts = gameAccountRepo.Accounts });
        }


        public async Task<IActionResult> GameAccounts()
        {
            return View("GameAccounts/List", gameAccountRepo.Accounts);
        }


        public async Task<IActionResult> ClientAccounts()
        {
            return View("ClientAccounts/List", userAccountRepo.Accounts(Constants.ClientRole));
        }


        public async Task<IActionResult> AdminAccounts()
        {
            return View("AdminAccounts/List", userAccountRepo.Accounts(Constants.AdminRole));
        }


    }
}
