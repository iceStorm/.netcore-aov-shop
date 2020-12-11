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
            return View(new DashboardViewModel {
                GameAccounts = gameAccountRepo.Accounts,
                ClientAccounts = userAccountRepo.Accounts(Constants.ClientRole)
            });
        }



        #region GAME ACCOUNT MANAGING
        public IActionResult GameAccounts()
        {
            return View("GameAccounts/List", gameAccountRepo.Accounts);
        }

        public IActionResult Import()
        {
            return View("GameAccounts/Import", new GameAccount());
        }

        [HttpPost]
        public IActionResult Import(GameAccount model)
        {
            if (ModelState.IsValid)
            {
                var currentAcc = gameAccountRepo.Accounts.Where(acc => acc.LoginName == model.LoginName).FirstOrDefault();
                if (currentAcc == null)
                {
                    gameAccountRepo.SaveGameAccount(model);
                    TempData["message"] = "Thêm Tài khoản thành công !";
                    return View("GameAccounts/Import", new GameAccount());
                }
                else
                {
                    TempData["message"] = "Tài khoản đã tồn tại !";
                }
            }
            else
            {
                TempData["message"] = "Vui lòng nhập đủ thông tin";
            }

            return View("GameAccounts/Import", model);
        }

        public IActionResult Delete(string accLoginName)
        {
            var getAcc = gameAccountRepo.Accounts.Where(acc => acc.LoginName == accLoginName).FirstOrDefault();
            if (getAcc != null)
            {
                gameAccountRepo.DeleteGameAccount(getAcc.LoginName);
                TempData["message"] = "Xoá Tài khoản thành công !";
                return RedirectToAction(nameof(GameAccounts));
            }

            TempData["message"] = "Tài khoản không tồn tại !";
            return RedirectToAction(nameof(GameAccounts));
        }

        public IActionResult Edit(string accLoginName)
        {
            var foundAcc = gameAccountRepo.Accounts.Where(acc => acc.LoginName == accLoginName).FirstOrDefault();
            if (foundAcc != null)
            {
                return View("GameAccounts/Edit", foundAcc);
            }


            TempData["message"] = "Tài khoản không tồn tại";
            return RedirectToAction(nameof(GameAccounts));
        }

        [HttpPost]
        public IActionResult Edit(GameAccount model)
        {
            if (ModelState.IsValid)
            {
                var foundAcc = gameAccountRepo.Accounts.FirstOrDefault(acc => acc.LoginName == model.LoginName);
                if (foundAcc != null)
                {
                    gameAccountRepo.SaveGameAccount(model);
                    TempData["message"] = "Cập nhật thành công !";
                    return RedirectToAction(nameof(GameAccounts));
                }


                TempData["message"] = "Tài khoản không tồn tại";
                return RedirectToAction(nameof(GameAccounts));
            }
            else
            {
                TempData["message"] = "Vui lòng nhập đủ thông tin";
                return View("GameAccounts/Edit", model);
            }
        }
        #endregion



        #region CLIENT ACCOUNT MANAGING
        public async Task<IActionResult> ClientAccounts()
        {
            return View("ClientAccounts/List", userAccountRepo.Accounts(Constants.ClientRole));
        }
        #endregion



        #region ADMIN ACCOUNT MANAGING
        public async Task<IActionResult> AdminAccounts()
        {
            return View("AdminAccounts/List", userAccountRepo.Accounts(Constants.AdminRole));
        }


        #endregion


    }
}
