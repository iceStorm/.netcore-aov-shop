using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Infrastructures;
using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<UserAccount> userManager;
        private SignInManager<UserAccount> signInManager;

        public AccountController(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }



        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Redirect(returnUrl);
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            return View(returnUrl);
        }

        public async Task<IActionResult> Profile()
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            return View(currentUser);
        }



        #region Login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                // check if user is existed by provided email
                var user = await userManager.FindByEmailAsync(viewModel.Email);
                if (user != null)
                {

                    // user existed --- let's login
                    var loginResult = await signInManager.PasswordSignInAsync(user, viewModel.Password, false, true);
                    if (loginResult.Succeeded)  // when the login succeeded
                    {
                        if (await userManager.IsInRoleAsync(user, Constants.ClientRole))
                            return Redirect("/");
                        else
                            return Redirect("/Admin");
                    }
                    else
                    {
                        if (loginResult.IsNotAllowed)
                            TempData["message"] = "Tài khoản chưa được xác thực.";
                        else
                        {
                            TempData["message"] = "Mật khẩu không chính xác.";
                        }
                    }
                }
                else
                {
                    TempData["message"] = "E-mail chưa được đăng ký";
                }
            }
            else
            {
                TempData["message"] = "Thông tin Đăng nhập không hợp lệ";
            }


            return View(viewModel);
        }
        #endregion



        #region SignUp
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByEmailAsync(viewModel.Email);
                if (user != null)
                {
                    TempData["message"] = "E-mail đã tồn tại";
                }
                else
                {

                    // init new Client model --> create
                    user = new UserAccount {
                        UserName = viewModel.Email,
                        Email = viewModel.Email,
                        SurName = viewModel.User.SurName,
                        FirstName = viewModel.User.FirstName
                    };

                    var createResult = await userManager.CreateAsync(user, viewModel.Password);
                    if (createResult.Succeeded)
                    {
                        TempData["message"] = "Vui lòng kiểm tra E-mail dể kích hoạt tài khoản !";
                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
                        if (createResult.Errors.Any(err => err.Code == "PasswordTooShort"))
                            TempData["message"] = "Mật khẩu phải có ít nhất 6 kí tự";
                        else
                        {
                            TempData["message"] = "Có lỗi trong quá trình Đăng ký";
                        }
                    }
                }
            }
            else
            {
                TempData["message"] = "Thông tin Đăng ký không hợp lệ";
            }


            return View(viewModel);
        }
        #endregion


    }
}
