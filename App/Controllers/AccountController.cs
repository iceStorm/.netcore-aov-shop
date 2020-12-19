using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using App.Infrastructures;
using App.Models;
using App.Repositories.Interfaces;
using App.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace App.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IHostingEnvironment environment;
        private IUserAccountRepo userAccountRepo;
        private MailkitMetaData mailkitMetadata;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<UserAccount> userManager;
        private SignInManager<UserAccount> signInManager;

        public AccountController(IHostingEnvironment environment,
            IUserAccountRepo userAccountRepo, MailkitMetaData mailkitMetadata,
            RoleManager<IdentityRole> roleManager, UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager)
        {
            this.environment = environment;
            this.userAccountRepo = userAccountRepo;
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
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            return View(currentUser);
        }


        [HttpPost]
        public async Task<IActionResult> Profile(UserAccount model)
        {
            if (ModelState.IsValid)
            {
                var saveResult = userAccountRepo.SaveAccount(model);
                if (saveResult != null)
                {
                    TempData["message"] = "Đã cập nhật thông tin";
                    return View(saveResult);
                }


                TempData["message"] = "Có lỗi trong quá trình xử lý";
                return View(model);
            }

            TempData["message"] = "Vui lòng nhập đủ thông tin";
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UploadAvatar()
        {
            var files = HttpContext.Request.Form.Files;
            if (files != null)
            {
                try
                {
                    // Getting FileName
                    var fileName = ContentDispositionHeaderValue.Parse(files[0].ContentDisposition).FileName.Trim('"');

                    // Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);



                    // Generate an unique string
                    var uniqueFileName = Convert.ToString(Guid.NewGuid());

                    // concating  FileName + FileExtension
                    var newFileName = uniqueFileName + fileExtension;



                    // Combines two strings into a path.
                    fileName = Path.Combine(environment.WebRootPath, "images", "user-avatars", "tmp", newFileName);


                    // Save the image to folder
                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        files[0].CopyTo(fs);
                        fs.Flush();
                    }


                    return Json(Path.Combine("images", "user-avatars", "tmp", newFileName));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }


            return BadRequest(Json("Error during upload"));
        }


        [HttpDelete]
        public async Task<IActionResult> RemoveAvatar()
        {
            var files = HttpContext.Request.Form.Files;
            return Json(true);
        }



        [Authorize(Roles = Constants.ClientRole)]
        public async Task<IActionResult> BoughtHistory()
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var fullyUser = userAccountRepo.Accounts(Constants.ClientRole).FirstOrDefault(acc => acc.Email == currentUser.Email);
            return View(fullyUser);
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
                            return Redirect(viewModel.ReturnUrl ?? "/");
                        else
                        {
                            if (viewModel.ReturnUrl != null)
                            {
                                Redirect(viewModel.ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("GameAccounts",  "Admin");
                            }
                        }
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

                    // init new Client model --> create  --- here the client is null
                    client = new UserAccount {
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
                                    Subject = "aov-shop.tk - Xác minh Tài khoản",
                                    Content = confirmationUrl,
                                    Sender = new MailboxAddress(mailkitMetadata.Sender),
                                    Receiver = new MailboxAddress(client.Email)
                                };



                                using (SmtpClient smtpClient = new SmtpClient())
                                {
                                    smtpClient.Connect(mailkitMetadata.SmtpServer, mailkitMetadata.Port);
                                    smtpClient.Authenticate(mailkitMetadata.UserName, mailkitMetadata.Password);
                                    smtpClient.Send(emailMessage.GetConfirmEmailMimeMessage());
                                    smtpClient.Disconnect(true);
                                }


                                TempData["message"] = "Vui lòng kiểm tra E-mail để kích hoạt !";
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


        #region Change Password
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);

                // check whether the old password that user entered matchs with current password stored in the database.
                var isOldPasswordSatisfied = await userManager.CheckPasswordAsync(currentUser, viewModel.OldPassword);
                if (!isOldPasswordSatisfied)
                {
                    TempData["message"] = "Mật khẩu cũ không đúng";
                    return View(viewModel);
                }
                else
                {
                    var changePasswordResult = await userManager.ChangePasswordAsync(currentUser, viewModel.OldPassword, viewModel.NewPassword);
                    if (changePasswordResult.Succeeded)
                    {
                        TempData["message"] = "Đã thay đổi Mật khẩu";
                        return RedirectToAction(nameof(Profile));
                    }


                    TempData["message"] = "Có lỗi trong quá trình xử lý";
                    return View(viewModel);
                }
            }


            TempData["message"] = "Vui lòng nhập đủ thông tin";
            return View(viewModel);
        }
        #endregion


        #region Reset Password
        public async Task<IActionResult> ResetPassword()
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                await sendResetPasswordEmail(currentUser);

                TempData["message"] = "Vui lòng kiểm tra E-mail để Reset";
                return RedirectToAction(nameof(ChangePassword));
            }
            catch (Exception ex)
            {
                TempData["message"] = "Có lỗi trong quá trình xử lý";
                return RedirectToAction(nameof(ChangePassword));
            }
        }


        private async Task sendResetPasswordEmail(UserAccount currentUser)
        {
            // generate new token string
            var tokenString = await userManager.GeneratePasswordResetTokenAsync(currentUser);


            // generate url from action -- controller -- route values
            var confirmationUrl = Url.Action(
                nameof(ResetPasswordEmail), "Account",
                new { token = tokenString, email = currentUser.Email },
                Request.Scheme
            );


            /* create the mail content */
            var emailMessage = new MailkitMessage
            {
                Subject = "aov-shop.tk - Đặt lại Mật khẩu",
                Content = confirmationUrl,
                Sender = new MailboxAddress(mailkitMetadata.Sender),
                Receiver = new MailboxAddress(currentUser.Email)
            };



            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Connect(mailkitMetadata.SmtpServer, mailkitMetadata.Port);
                smtpClient.Authenticate(mailkitMetadata.UserName, mailkitMetadata.Password);
                smtpClient.Send(emailMessage.GetResetPasswordMimeMessage());
                smtpClient.Disconnect(true);
            }
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return View(new ResetPasswordEmailViewModel { Token = token, UserId = user.Id });
            }


            TempData["message"] = "Người dùng không tồn tại";
            return View(RedirectToAction("Index", "Home"));
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordEmail(ResetPasswordEmailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(viewModel.UserId);
                if (user != null)
                {
                    var resetPasswordResullt = await userManager.ResetPasswordAsync(user, viewModel.Token, viewModel.NewPassword);
                    if (resetPasswordResullt.Succeeded)
                    {
                        TempData["message"] = "Đã thay đổi mật khẩu";


                        // if the user is signed-in
                        if (signInManager.IsSignedIn(HttpContext.User))
                        {
                            return RedirectToAction(nameof(Profile));
                        }


                        // the user is not signed-in
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        if (resetPasswordResullt.Errors.Any(err => err.Code == "InvalidToken"))
                        {
                            TempData["message"] = "E-mail Token đã hết hiệu lực";
                            return View(viewModel);
                        }

                        if (resetPasswordResullt.Errors.Any(err => err.Code == "PasswordTooShort"))
                        {
                            TempData["message"] = "Mật khẩu ít nhất 6 kí tự";
                            return View(viewModel);
                        }


                        TempData["message"] = "Có lỗi trong quá trình xử lý";
                        return View(viewModel);
                    }
                }


                TempData["message"] = "Người dùng không tồn tại";
                return RedirectToAction("Index", "Home");
            }



            TempData["message"] = "Vui lòng nhập đủ thông tin";
            return View(viewModel);
        }
        #endregion


        #region Reset Password without Signed-in
        [AllowAnonymous]
        public IActionResult DoResetPassword()
        {
            ViewBag.Email = null;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DoResetPassword(string email)
        {
            ViewBag.Email = email;


            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    try
                    {
                        await sendResetPasswordEmail(user);
                        TempData["message"] = "Vui lòng kiểm tra E-mail để Reset";
                        return RedirectToAction(nameof(Login));
                    }
                    catch (Exception ex)
                    {
                        TempData["message"] = "Có lỗi trong quá trình xử lý";
                        return View();
                    }
                }


                TempData["message"] = "E-mail chưa được Đăng ký";
                return View();
            }


            TempData["message"] = "Vui lòng nhập đủ thông tin";
            return View();
        }
        #endregion

    }
}
 