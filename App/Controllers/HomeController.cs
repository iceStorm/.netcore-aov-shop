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
        private IGameAccountRepo gameAccountRepo;
        private IUserAccountRepo userAccountRepo;
        private int itemsPerPage = 8;

        public HomeController(UserManager<UserAccount> userManager, IGameAccountRepo accRepo, IUserAccountRepo userAccountRepo)
        {
            this.userManager = userManager;
            this.gameAccountRepo = accRepo;
            this.userAccountRepo = userAccountRepo;
        }


        [HttpGet(Name = "Get by Rank")]
        public IActionResult Index(int rankId = -1, int pageIndex = 1)
        {
            var viewModel = new HomeViewModel
            {
                AccountsList = gameAccountRepo.Accounts
                    .Where(acc => acc.UserAccountId == null)
                    .Where(acc => rankId == -1 || acc.Rank.Id == rankId)
                    .OrderByDescending(acc => acc.Price)
                    .Skip((pageIndex - 1) * itemsPerPage)
                    .Take(itemsPerPage),

                PagingInfo = new PagingInfo
                {
                    CurrentRankId = rankId,
                    ItemsPerPage = itemsPerPage,
                    CurrentPageIndex = pageIndex,
                    TotalItems = gameAccountRepo.Accounts
                        .Where(acc => acc.UserAccountId == null)
                        .Where(acc => rankId == -1 || acc.Rank.Id == rankId).Count(),
                }
            };


            return View(viewModel);
        }

        [HttpGet(Name = "Get by Price")]
        public IActionResult Price(int from = 0, int to = 999999999, int pageIndex = 1)
        {
            int rankId = -1;

            var viewModel = new HomeViewModel
            {
                AccountsList = gameAccountRepo.Accounts
                    .Where(acc => acc.UserAccountId == null)
                    .Where(acc => rankId == -1 || acc.Rank.Id == rankId)
                    .Where(acc => acc.Price >= from && acc.Price <= to)
                    .OrderBy(acc => acc.Price)
                    .Skip((pageIndex - 1) * itemsPerPage)
                    .Take(itemsPerPage),

                PagingInfo = new PagingInfo
                {
                    CurrentRankId = rankId,
                    ItemsPerPage = itemsPerPage,
                    CurrentPageIndex = pageIndex,
                    TotalItems = gameAccountRepo.Accounts
                        .Where(acc => acc.UserAccountId == null)
                        .Where(acc => rankId == -1 || acc.Rank.Id == rankId)
                        .Where(acc => acc.Price >= from && acc.Price <= to)
                        .Count()
                }
            };


            return View("Index", viewModel);
        }



        public IActionResult GameAccountDetail(int accId)
        {
            var acc = gameAccountRepo.Accounts.FirstOrDefault(acc => acc.Id == accId && acc.UserAccountId == null);
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
                var selectedAcc = gameAccountRepo.Accounts.FirstOrDefault(acc => acc.Id == accId && acc.UserAccountId == null);
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
                    var selectedAcc = gameAccountRepo.Accounts.FirstOrDefault(acc => acc.Id == accId);
                    var currentUser = await userManager.GetUserAsync(HttpContext.User);


                    if (selectedAcc != null && currentUser != null)
                    {

                        if (selectedAcc.UserAccountId != null)
                        {
                            TempData["message"] = "Rất tiếc, Tài khoản đã được mua trước đó";
                            return RedirectToAction(nameof(Index));
                        }


                        selectedAcc.UserAccountId = currentUser.Id;
                        gameAccountRepo.SaveGameAccount(selectedAcc);


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
