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
    [Roles (Constants.AdminRole, Constants.SuperAdminRole)]
    public class AdminController : Controller
    {
        private IRankRepo rankRepo;
        private IGameAccountRepo gameAccountRepo;
        private IUserAccountRepo userAccountRepo;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<UserAccount> userManager;
        private SignInManager<UserAccount> signInManager;

        public AdminController(IRankRepo rankRepo, IGameAccountRepo gameAccountRepo, 
            IUserAccountRepo userAccountRepo, RoleManager<IdentityRole> roleManager, 
            UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager)
        {
            this.rankRepo = rankRepo;
            this.gameAccountRepo = gameAccountRepo;
            this.userAccountRepo = userAccountRepo;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }



        #region GAME ACCOUNT MANAGING
        public IActionResult GameAccounts(string accLoginName = null)
        {
            return View("GameAccounts/List", accLoginName == null ? 
                gameAccountRepo.Accounts :
                gameAccountRepo.Accounts.Where(acc => acc.LoginName == accLoginName)
            );
        }



        public IActionResult Import()
        {
            return View("GameAccounts/Import", new AddGameAccountViewModel { RanksList = rankRepo.Ranks });
        }

        [HttpPost]
        public IActionResult Import(AddGameAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (rankRepo.Ranks.FirstOrDefault(r => r.Name == model.RankName) == null)
                {
                    model.RanksList = rankRepo.Ranks;
                    TempData["message"] = "Rank không hợp lệ";
                    return View("GameAccounts/Import", model);
                }


                var currentAcc = gameAccountRepo.Accounts.Where(acc => acc.LoginName == model.LoginName).FirstOrDefault();
                if (currentAcc == null)
                {
                    gameAccountRepo.SaveGameAccount(model);
                    TempData["message"] = "Thêm Tài khoản thành công !";
                    return View("GameAccounts/Import", new AddGameAccountViewModel { RanksList = rankRepo.Ranks });
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


            model.RanksList = rankRepo.Ranks;
            return View("GameAccounts/Import", model);
        }



        public IActionResult Delete(int accId)
        {
            var getAcc = gameAccountRepo.Accounts.Where(acc => acc.Id == accId).FirstOrDefault();
            if (getAcc != null)
            {
                gameAccountRepo.DeleteGameAccount(getAcc.LoginName);
                TempData["message"] = "Xoá Tài khoản thành công !";
                return RedirectToAction(nameof(GameAccounts));
            }

            TempData["message"] = "Tài khoản không tồn tại !";
            return RedirectToAction(nameof(GameAccounts));
        }



        public IActionResult Edit(int accId)
        {
            var foundAcc = gameAccountRepo.Accounts.Where(acc => acc.Id == accId).FirstOrDefault();
            if (foundAcc != null)
            {
                var viewModel = new AddGameAccountViewModel { RanksList = rankRepo.Ranks };
                viewModel.CopyValues(foundAcc);
                viewModel.Id = foundAcc.Id;

                
                return View("GameAccounts/Edit", viewModel);
            }


            TempData["message"] = "Tài khoản không tồn tại";
            return RedirectToAction(nameof(GameAccounts));
        }

        [HttpPost]
        public IActionResult Edit(AddGameAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var isDuplicated = gameAccountRepo.Accounts.FirstOrDefault(acc
                    => acc.LoginName == viewModel.LoginName
                        && acc.Id != viewModel.Id);


                if (isDuplicated == null)
                {
                    gameAccountRepo.SaveGameAccount(viewModel);
                    TempData["message"] = "Cập nhật thành công !";
                    return RedirectToAction(nameof(GameAccounts));
                }


                TempData["message"] = "Tài khoản bị trùng hoặc không tồn tại";
                return RedirectToAction(nameof(GameAccounts));
            }
            else
            {
                viewModel.RanksList = rankRepo.Ranks;
                TempData["message"] = "Vui lòng nhập đủ thông tin";
                return View("GameAccounts/Edit", viewModel);
            }
        }
        #endregion



        #region CLIENT ACCOUNT MANAGING
        public async Task<IActionResult> ClientAccounts()
        {
            return View("ClientAccounts/List", userAccountRepo.Accounts(Constants.ClientRole));
        }

        public IActionResult Buyer(string userId)
        {
            var user = userAccountRepo.
                Accounts(Constants.ClientRole)
                    .FirstOrDefault(client => client.Id == userId);

            if (user != null)
                return View(user);


            TempData["message"] = "Người dùng không tồn tại";
            return RedirectToAction(nameof(GameAccounts));
        }
        #endregion



        #region ADMIN ACCOUNT MANAGING
        [Authorize (Roles = Constants.SuperAdminRole)]
        public async Task<IActionResult> AdminAccounts()
        {
            return View("AdminAccounts/List", userAccountRepo.Accounts(Constants.AdminRole));
        }


        public IActionResult DetailInfo(string userId)
        {
            var user = userAccountRepo.
                Accounts(Constants.AdminRole)
                    .FirstOrDefault(ad => ad.Id == userId);

            if (user != null)
                return View("Buyer", user);


            TempData["message"] = "Người dùng không tồn tại";
            return RedirectToAction(nameof(AdminAccounts));
        }



        public IActionResult AddAdmin()
        {
            return View("AdminAccounts/Add", new SignUpViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(SignUpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(viewModel.Email);
                if (user == null)
                {
                    viewModel.User.Email = viewModel.Email;
                    viewModel.User.EmailConfirmed = true;

                    var createResult = await userManager.CreateAsync(viewModel.User, viewModel.Password);
                    if (createResult.Succeeded)
                    {
                        var roleAddingResult = await userManager.AddToRoleAsync(viewModel.User, Constants.AdminRole);
                        if (roleAddingResult.Succeeded)
                        {
                            TempData["message"] = "Đã tạo tài khoản thành công !";
                            return RedirectToAction(nameof(AdminAccounts));
                        }
                    }

                    TempData["message"] = "Có lỗi trong quá trình xử lý";
                    return View("AdminAccounts/Add", viewModel);
                }


                TempData["message"] = "E-mail đã được Đăng ký";
                return View("AdminAccounts/Add", viewModel);
            }


            TempData["message"] = "Vui lòng nhập đủ thông tin";
            return View("AdminAccounts/Add", viewModel);
        }



        public IActionResult DeleteAdmin(string userId)
        {
            var user = userAccountRepo.Accounts(Constants.AdminRole).FirstOrDefault(acc => acc.Id == userId);
            if (user != null)
            {
                if (userAccountRepo.DeleteAccount(user))
                {
                    TempData["message"] = "Đã xoá tài khoản Admin";
                    return RedirectToAction(nameof(AdminAccounts));
                }


                TempData["message"] = "Có lỗi trong quá trình xử lý";
                return RedirectToAction(nameof(AdminAccounts));
            }


            TempData["message"] = "Người dùng không tồn tại";
            return RedirectToAction(nameof(AdminAccounts));
        }
        #endregion


    }
}
