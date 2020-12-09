using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Infrastructures;
using App.Repositories;
using App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{

    [Authorize (Roles = Constants.ClientRole)]
    public class HomeController : Controller
    {
        private IGameAccountRepo accRepo;

        public HomeController(IGameAccountRepo accRepo)
        {
            this.accRepo = accRepo;
        }



        [AllowAnonymous]
        [Authorize(Roles = Constants.ClientRole)]
        public IActionResult Index()
        {
            return View(accRepo.Accounts);
        }


        public IActionResult Buy()
        {
            return View(accRepo.Accounts);
        }

    }
}
