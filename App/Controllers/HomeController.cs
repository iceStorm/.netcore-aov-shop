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
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{

    public class HomeController : Controller
    {
        private IGameAccountRepo accRepo;
        private int itemsPerPage = 8;

        public HomeController(IGameAccountRepo accRepo)
        {
            this.accRepo = accRepo;
        }



        [AllowAnonymous]
        public IActionResult Index(int pageIndex = 1)
        {
            return View(new HomeViewModel {
                AccountsList = accRepo.Accounts
                    .Skip((pageIndex - 1) * itemsPerPage)
                    .Take(itemsPerPage),

                PagingInfo = new PagingInfo
                {
                    CurrentPageIndex = pageIndex,
                    TotalItems = accRepo.Accounts.Count(),
                    ItemsPerPage = itemsPerPage
                }
            });
        }


        public IActionResult GameAccountDetail(string accLoginName)
        {
            return View(accRepo.Accounts.FirstOrDefault(acc => acc.LoginName == accLoginName));
        }


        [HttpPost]
        [Authorize(Roles = Constants.ClientRole)]
        public IActionResult Buy()
        {
            return View();
        }


    }
}
