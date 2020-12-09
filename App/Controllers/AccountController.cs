using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Infrastructures;
using App.Models;
using App.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace App.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private MailkitMetaData mailkitMetadata;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<UserAccount> userManager;
        private SignInManager<UserAccount> signInManager;

        public AccountController(MailkitMetaData mailkitMetadata, 
            RoleManager<IdentityRole> roleManager, UserManager<UserAccount> userManager, 
            SignInManager<UserAccount> signInManager)
        {
            this.mailkitMetadata = mailkitMetadata;
            this.roleManager = roleManager;
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
            TempData["returnUrl"] = returnUrl;
            return View();
        }



        #region Main functions
        public async Task<IActionResult> Profile()
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            return View(currentUser);
        }


        /*[Authorize(Roles = Constants.ClientRole)]
        public async Task<IActionResult> ReCharge()
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            return View(currentUser);
        }*/


        [Authorize(Roles = Constants.ClientRole)]
        public async Task<IActionResult> BoughtHistory()
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            return View(currentUser);
        }
        #endregion





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
                            return Redirect("/Dashboard");
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

                var client = await userManager.FindByEmailAsync(viewModel.Email);
                if (client != null)
                {
                    TempData["message"] = "E-mail đã tồn tại";
                }
                else
                {

                    // init new Client model --> create
                    client = new UserAccount {
                        UserName = viewModel.Email,
                        Email = viewModel.Email,
                        SurName = viewModel.User.SurName,
                        FirstName = viewModel.User.FirstName
                    };


                    var createResult = await userManager.CreateAsync(client, viewModel.Password);
                    if (createResult.Succeeded)
                    {
                        var roleExisted = true;

                        /* check and add new role if needed */
                        if (await roleManager.RoleExistsAsync(Constants.ClientRole) == false)
                        {
                            var createRoleResult = await roleManager.CreateAsync(new IdentityRole(Constants.ClientRole));
                            roleExisted = createRoleResult.Succeeded == true;
                        }



                        if (roleExisted)
                        {
                            var addRoleResult = await userManager.AddToRoleAsync(client, Constants.ClientRole);
                            if (addRoleResult.Succeeded)
                            {

                                /* generate confirmationUrl that will be contained in the email */
                                var tokenString = await userManager.GenerateEmailConfirmationTokenAsync(client);
                                var confirmationUrl = Url.Action(
                                    nameof(ConfirmEmail),
                                    "Account",
                                    new { token = tokenString, email = client.Email },
                                    Request.Scheme
                                );


                                /* send the mail */
                                var emailMessage = new MailkitMessage
                                {
                                    Subject = "ShopAOV.vn - Xác minh Tài khoản",
                                    Content = confirmationUrl,
                                    Sender = new MailboxAddress(mailkitMetadata.Sender),
                                    Receiver = new MailboxAddress(client.Email)
                                };



                                using (SmtpClient smtpClient = new SmtpClient())
                                {
                                    smtpClient.Connect(mailkitMetadata.SmtpServer, mailkitMetadata.Port, true);
                                    smtpClient.Authenticate(mailkitMetadata.UserName, mailkitMetadata.Password);
                                    smtpClient.Send(emailMessage.GetMimeMessage());
                                    smtpClient.Disconnect(true);
                                }


                                TempData["message"] = "Vui lòng kiểm tra E-mail để kích hoạt tài khoản !";
                                return RedirectToAction(nameof(Login));
                            }   //  addRole

                        }   // roleExisted


                        TempData["message"] = "Có lỗi trong quá trình Đăng ký !";
                        return View();
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


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {

            var client = await userManager.FindByEmailAsync(email);
            if (client != null)
            {
                var confirmResult = await userManager.ConfirmEmailAsync(client, token);
                if (confirmResult.Succeeded)
                {
                    return View(true);
                }
            }


            return View(false);
        }
        #endregion


    }
}
