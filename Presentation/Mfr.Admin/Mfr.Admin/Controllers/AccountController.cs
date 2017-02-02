using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mfr.Admin.Models.Account;
using Mfr.Core.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
namespace Mfr.Admin.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        public AccountController()
        {
        }
        public ActionResult SomeImage(string imageName)
        {
            if (imageName == null)
                return File("~/Content/img/NoProfilePicture.png",
                                "image/jpeg");

            var root = @"~/App_Themes/Images/";
            var path = Path.Combine(root, imageName);
            if (!path.StartsWith(root))
            {
                // Ensure that we are serving file only inside the root folder
                // and block requests outside like "../web.config"
                throw new HttpException(403, "Forbidden");
            }

            return File(path, "image/jpeg");
        }
        public AccountController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }



        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
     
        
        //GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        //
        [AllowAnonymous]
        public ActionResult Confirm(string email)
        {
            ViewBag.Email = email; return View();
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model,
            HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.Name,
                    LastName = model.FamilyName,
                    AvatarUrl = model.AvatarUrl,
                    BirthDate = model.BirthDate.ToString(CultureInfo.InvariantCulture),
                    PhoneNumber = model.Phonenumber
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Member");
                    //string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");
                    SendEmail(user, model.Password);
                    if (file != null)
                    {
                        try
                        {
                            var fileName = Guid.NewGuid().ToString()+".jpg";
                            var path = Path.Combine(Server.MapPath("~/App_Themes/Images"),
                                            fileName);
                            file.SaveAs(path);
                            user.AvatarUrl = fileName;
                            await UserManager.UpdateAsync(user);
                        }
                        catch
                        {

                            ModelState.AddModelError("Picture", "There was an error uploading the image!");
                        }
                    }
                    return RedirectToAction("Confirm", "Account", new { email = user.Email });
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        private void SendEmail(ApplicationUser user, string pass)
        {
            var m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress("{Your Email}", "Web Registration"),
                new System.Net.Mail.MailAddress(user.Email))
            {
                Subject = "Confirm E-Mail",
                Body =
                    string.Format(
                        " Dear {0}<BR/>Thank you for Registering in our web Site please Click the link below to complete registeration: <BR/><a href=\"{1}\" title=\"User Email Confirm\">{1}</a>",
                        user.UserName,
                        Url.Action("ConfirmEmail", "Account", new { token = user.Id, email = user.Email, password = pass },
                            Request.Url?.Scheme)),
                IsBodyHtml = true
            };

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com")
            {
                Credentials = new System.Net.NetworkCredential("[YOUR-Email]", "[YOUR PASSWORD]"),
                EnableSsl = true
            };
            smtp.Send(m);

        }
        private void SendEmail(int userId, string title, string body)
        {
            var user = UserManager.FindById(userId);
            var m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress("[Your EMAIL]", "PasswordReset"),
                new System.Net.Mail.MailAddress(user.Email))
            {
                Body = body,
                IsBodyHtml = true,
                Subject = title
            };

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com")
            {
                Credentials = new System.Net.NetworkCredential("[You're E-Mail]", "[YOUR PASSWORD]"),
                EnableSsl = true
            };
            smtp.Send(m);
        }

        //Check For Existence Of user inputed,for remote Mehtod in ViewModel
        //Post
        [HttpPost]
        public JsonResult DoesUserNameExist(string userName)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == userName);

            return Json(user == null);
        }
        //Check For Existence OF email inputed, for remote Mehtod in ViewModel
        //Post
        [HttpPost]
        public JsonResult DoesEmailExist(string email)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Email == email);
            return Json(user == null);
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            // var user = await UserManager.FindByNameAsync(model.Email);
            var user = UserManager.Find(model.UserName, model.Password);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    //string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account-Resend");
                    SendEmail(user, model.Password);
                    // Uncomment to debug locally  
                    // ViewBag.Link = callbackUrl;
                    ViewBag.errorMessage = "You must First Confirm the E-Mail. "
                                         + "Confirmation E-Mail Has been sended to you again.";
                    return View("Error");
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName,
                model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new
                    {
                        ReturnUrl = returnUrl,
                        RememberMe = model.RememberMe
                    });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //GET
        //Account/MainEdit
        [Authorize]
        public ActionResult MainEdit()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            var register = new EditViewModel
            {//Initialize ViewModel With Default Values
                UserName = user.UserName,
                // Email = user.Email,
                Name = user.FirstName,
                FamilyName = user.LastName,
                BirthDate = user.BirthDate,
                Phonenumber = user.PhoneNumber,

            };

            return View(register);
        }


        [Authorize]
        [HttpPost]
        //     [ValidateAntiForgeryToken]
        // POST: Account/Edit
        public async Task<ActionResult> _CredentialsEdit(EditViewModel model,
            HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                //user.Email = model.Email;
                user.BirthDate = model.BirthDate;
                user.FirstName = model.Name;
                user.LastName = model.FamilyName;
                user.PhoneNumber = model.Phonenumber;
                user.UserName = model.UserName;

                if (file != null)
                {
                    try
                    {
                        string fileName;
                        if (user.AvatarUrl != null)
                        {
                            //Delete Last Pic If It Exist
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/App_Themes/Images"), user.AvatarUrl));
                            fileName = user.AvatarUrl;
                        }
                        else
                        {
                            fileName = Guid.NewGuid().ToString()+".jpg";
                        }
                        var path = Path.Combine(Server.MapPath("~/App_Themes/Images"), fileName);
                        file.SaveAs(path);
                        user.AvatarUrl = fileName;
                    }
                    catch
                    {
                        ModelState.AddModelError("Picture", "There was a problem uploading the image!");
                    }
                }
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                //if we reach here something is wrong
                AddErrors(result);
            }

            return View();

        }

        [HttpPost]
        public async Task<ActionResult> _PasswordEdit(PasswordViewModel model)
        {
            //var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            var result =
                await
                    UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword,
                        model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            //if we reach here something is wrong
            ModelState.AddModelError("PasswordChange", result.Errors.FirstOrDefault());


            return View();
        }
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        //POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code,
                    isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }




        //GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int token, string email, string password)
        {
            ApplicationUser user = this.UserManager.FindById(token);
            if (user != null)
            {
                if (user.Email == email)
                {
                    user.EmailConfirmed = true;
                    await UserManager.UpdateAsync(user);
                    var result =
                        await SignInManager.PasswordSignInAsync(user.UserName,
                                password, false, shouldLockout: true);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            return RedirectToAction("Index", "Home", new { ConfirmedEmail = user.Email });
                        case SignInStatus.LockedOut:
                        // return View("Lockout");
                        case SignInStatus.RequiresVerification:
                        // return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            {
                                return View("Error");
                            }

                    }
                }
            }

            //if we got this far error is happening
            return View("Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                                new { userId = user.Id, code = code }, protocol: Request.Url?.Scheme);
                SendEmail(user.Id,
                          "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        ////
        //// GET: /Account/ForgotPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id,
                            model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }
        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == 0)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem
            {
                Text = purpose,
                Value = purpose
            }).ToList();
            return View(new SendCodeViewModel
            {
                Providers = factorOptions,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }


        //GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        ////
        //// POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}


        //// GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new AccountViewModels.ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        ////
        //// POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(AccountViewModels.ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        ////
        //// POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        ////
        //// GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Password", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
        #endregion


    }
}