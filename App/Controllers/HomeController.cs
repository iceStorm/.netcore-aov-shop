using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Infrastructures;
using App.Models;
using App.Repositories;
using App.Repositories.Interfaces;
using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{

    public class HomeController : Controller
    {
        private UserManager<UserAccount> userManager;
        private IGameAccountRepo accRepo;
        private IUserAccountRepo userAccountRepo;
        private int itemsPerPage = 8;

        public HomeController(UserManager<UserAccount> userManager, IGameAccountRepo accRepo, IUserAccountRepo userAccountRepo)
        {
            this.userManager = userManager;
            this.accRepo = accRepo;
            this.userAccountRepo = userAccountRepo;
        }



        public IActionResult Index(int pageIndex = 1)
        {
            return View(new HomeViewModel {
                AccountsList = accRepo.Accounts
                    .Where(acc => acc.UserAccountId == null)
                    .Skip((pageIndex - 1) * itemsPerPage)
                    .Take(itemsPerPage),

                PagingInfo = new PagingInfo
                {
                    CurrentPageIndex = pageIndex,
                    TotalItems = accRepo.Accounts.Where(acc => acc.UserAccountId == null).Count(),
                    ItemsPerPage = itemsPerPage
                }
            });
        }


        public IActionResult GameAccountDetail(int accId)
        {
            var acc = accRepo.Accounts.FirstOrDefault(acc => acc.Id == accId && acc.UserAccountId == null);
            if (acc != null)
            {
                return View(acc);
            }

            TempData["message"] = "Tài khoản Game không tồn tại";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Authorize(Roles = Constants.ClientRole)]
        public async Task<IActionResult> Buy(int accId)
        {
            if (ModelState.IsValid)
            {
                var selectedAcc = accRepo.Accounts.FirstOrDefault(acc => acc.Id == accId && acc.UserAccountId == null);
                var currentUser = await userManager.GetUserAsync(HttpContext.User);


                if (selectedAcc != null && currentUser != null)
                {
                    return View(new BuyViewModel { GameAccount = selectedAcc, Buyer = currentUser });
                }
            }


            TempData["message"] = "Tài khoản Game không tồn tại";
            return RedirectToAction(nameof(GameAccountDetail), accId);
        }



        [Authorize(Roles = Constants.ClientRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(int accId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selectedAcc = accRepo.Accounts.FirstOrDefault(acc => acc.Id == accId);
                    var currentUser = await userManager.GetUserAsync(HttpContext.User);


                    if (selectedAcc != null && currentUser != null)
                    {

                        if (selectedAcc.UserAccountId != null)
                        {
                            TempData["message"] = "Rất tiếc, Tài khoản đã được mua trước đó";
                            return RedirectToAction(nameof(Index));
                        }


                        selectedAcc.UserAccountId = currentUser.Id;
                        accRepo.SaveGameAccount(selectedAcc);


                        TempData["message"] = "Mua tài khoản Thành công !";
                        return View("BuyCompleted", new BuyViewModel { Buyer = currentUser, GameAccount = selectedAcc });
                    }
                    else
                    {
                        TempData["message"] = "Có lỗi trong quá trình Thanh toán";
                        return RedirectToAction(nameof(GameAccountDetail), accId);
                    }
                }
                catch (Exception ex)
                {
                    TempData["message"] = "Có lỗi trong quá trình Thanh toán";
                    return RedirectToAction(nameof(GameAccountDetail), accId);
                }
            }


            TempData["message"] = "Có lỗi trong quá trình Thanh toán";
            return RedirectToAction(nameof(GameAccountDetail), accId);
        }
       


        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult Guides()
        {
            return View();
        }

    }
}
