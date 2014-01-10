using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using CampusBookFlip.WebUI.Filters;
using CampusBookFlip.WebUI.Models;
using CampusBookFlip.Domain.Abstract;
using PoliteCaptcha;
using Postal;
using CampusBookFlip.Domain.Entities;
using CampusBookFlip.WebUI.Abstract;
using System.Text.RegularExpressions;
using CampusBookFlip.WebUI.Infrastructure;

namespace CampusBookFlip.WebUI.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private IRepository repo;
        private IEmailService eservice;
        private ICBFSecurity secure;

        public AccountController(IRepository repo, IEmailService eservice, ICBFSecurity secure)
        {
            this.repo = repo;
            this.eservice = eservice;
            this.secure = secure;
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
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && secure.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            secure.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateSpamPrevention]
        public ActionResult Register(RegisterModel model)
        {
            if (repo.User.Where(u => u.EmailAddress.ToLower() == model.EmailAddress.ToLower()).Count() > 0)
            {
                ModelState.AddModelError("", "This email is already registered with another account.");
            }
            string regex = @"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$";
            Regex r = new Regex(regex);
            if (r.IsMatch(model.UserName))
            {
                ModelState.AddModelError("", "Please use something other than an email for your user name.");
            }
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    string confirmationToken = secure.CreateUserAndAccount(model.UserName, model.Password, new
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Paid = false,
                        EmailAddress = model.EmailAddress
                    }, requireConfirmationToken: true);
                    string sharedSecret = secure.NewToken;
                    confirmationToken = secure.EncryptStringAES(confirmationToken, sharedSecret);
                    var email = new CampusBookFlip.WebUI.Models.ConfirmTokenEmail
                    {
                        To = model.EmailAddress,
                        FirstName = model.FirstName,
                        ConfirmationToken = confirmationToken,
                        SharedSecret = sharedSecret,
                        From = CampusBookFlip.WebUI.Infrastructure.Constants.EMAIL_NO_REPLY,
                        Subject = CampusBookFlip.WebUI.Infrastructure.Constants.COMPLETE_REGISTRATION_PROCESS,
                    };
                    eservice.Send(email);
                    return RedirectToAction("RegisterStepTwo", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult RegisterStepTwo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegisterConfirmation(string token, string secret)
        {
            token = secure.DecryptStringAES(token, secret);
            if (secure.ConfirmAccount(token))
            {
                return RedirectToAction("ConfirmationSuccess");
            }
            return RedirectToAction("ConfirmationFailure");
        }

        [AllowAnonymous]
        public ActionResult ConfirmationSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmationFailure()
        {
            return View();
        }

        [AllowAnonymous]
        public ViewResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model) 
        {
            string regex = @"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$";
                Regex r = new Regex(regex);
            if (!secure.UserExists(model.UsernameOrEmail) && repo.User.Count(u => u.EmailAddress == model.UsernameOrEmail) == 0)
            {
                if (r.IsMatch(model.UsernameOrEmail))
                {
                    ModelState.AddModelError("", "Could not find this email address in our records.");
                }
                else
                {
                    ModelState.AddModelError("", "This user name does not exist with us.");
                }
            }
            else
            {

                CBFUser user = r.IsMatch(model.UsernameOrEmail) ? repo.User.FirstOrDefault(u => u.EmailAddress == model.UsernameOrEmail) : 
                repo.User.FirstOrDefault(u => u.AppUserName == model.UsernameOrEmail);
                if (!secure.HasLocalAccount(user.Id))
                {
                    ModelState.AddModelError("", "You do not have a local password to reset. Try using the authentication provider you signed up with to log in.");
                }
            }
            

            if (ModelState.IsValid)
            {
                DateTime now = DateTime.Now;
                DateTime reset = now.AddDays(1);
                CBFUser user = repo.User.FirstOrDefault(u => u.EmailAddress == model.UsernameOrEmail || u.AppUserName == model.UsernameOrEmail);
                string sharedSecret = secure.NewToken;

                string confirmationToken = secure.EncryptStringAES(
                    plainText: secure.GeneratePasswordResetToken(user.AppUserName, (int)reset.Subtract(now).TotalMinutes),
                    sharedSecret: sharedSecret
                );
                
                var email = new ForgotPasswordEmail
                {
                    ConfirmationToken = confirmationToken,
                    SharedSecret = sharedSecret,
                    From = Constants.EMAIL_NO_REPLY,
                    To = user.EmailAddress,
                    FirstName = user.FirstName,
                    Subject = Constants.FORGOT_PASSWORD,
                    Expiration = reset
                };
                eservice.Send(email);
                return RedirectToAction("ForgotPasswordStepTwo");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult ForgotPasswordStepTwo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string token, string secret)
        {
            token = secure.DecryptStringAES(token, secret);
            int UserId = secure.GetUserIdFromPasswordResetToken(token);
            if (UserId >= 1)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel
                {
                    PasswordResetToken = token
                };
                return View(model);
            }
            return RedirectToAction("ResetPasswordFailure");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateSpamPrevention]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                int UserId = secure.GetUserIdFromPasswordResetToken(model.PasswordResetToken);
                CBFUser user = repo.User.FirstOrDefault(u => u.Id == UserId);
                if (secure.ResetPassword(model.PasswordResetToken, model.Password))
                {
                    secure.Login(user.AppUserName, model.Password);
                    return RedirectToAction("ResetPasswordSuccess");
                }
                else
                {
                    return RedirectToAction("ResetPasswordFailure");
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordFailure()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordSuccess()
        {
            return View();
        }

        public ViewResult ChangeEmail()
        {
            int id = secure.CurrentUserId;
            CBFUser user = repo.User.FirstOrDefault(u => u.Id == id);
            return View(new ChangeEmailViewModel
            {
                OldEmail = user.EmailAddress
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(ChangeEmailViewModel model)
        {
            if (repo.User.Where(u => u.EmailAddress.ToLower() == model.EmailAddress.ToLower()).Count() > 0)
            {
                ModelState.AddModelError("", "This email is already registered with another account.");
            }
            if (ModelState.IsValid)
            {
                string originalConfirmationToken = secure.NewToken;
                string sharedSecret = secure.NewToken;
                string confirmationToken = secure.EncryptStringAES(originalConfirmationToken, sharedSecret);

                int id = secure.CurrentUserId;
                CBFUser currentUser = repo.User.FirstOrDefault(c => c.Id == id);
                repo.SaveChangeEmailRequest(new ChangeEmailRequest
                {
                    ConfirmationToken = originalConfirmationToken,
                    NewEmail = model.EmailAddress,
                    Id = currentUser.Id
                });
                var email = new CampusBookFlip.WebUI.Models.NewEmailTokenEmail
                {
                    FirstName = currentUser.FirstName,
                    From = CampusBookFlip.WebUI.Infrastructure.Constants.EMAIL_NO_REPLY,
                    Subject = CampusBookFlip.WebUI.Infrastructure.Constants.CHANGE_EMAIL,
                    To = model.EmailAddress,
                    ConfirmationToken = confirmationToken,
                    SharedSecret = sharedSecret,
                    NewEmail = model.EmailAddress,
                    OldEmail = currentUser.EmailAddress
                };
                
                eservice.Send(email);
                return RedirectToAction("RegisterNewEmailStepTwo");
            }
            return View(model);
        }

        public ViewResult RegisterNewEmailStepTwo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegisterNewEmail(string token, string secret)
        {
            token = secure.DecryptStringAES(token, secret);
            ChangeEmailRequest request = repo.ChangeEmailRequest.FirstOrDefault(c => c.ConfirmationToken == token);
            if (request == null)
            {
                return RedirectToAction("ChangeEmailFailure");
            }
            CBFUser user = request.User;
            user.EmailAddress = request.NewEmail;
            repo.SaveUser(user);
            repo.DeleteChangeEmailRequest(request.Id);
            return RedirectToAction("ChangeEmailSuccess");
        }

        [AllowAnonymous]
        public ViewResult ChangeEmailFailure()
        {
            return View();
        }

        [AllowAnonymous]
        public ViewResult ChangeEmailSuccess()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = secure.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        secure.CreateAccount(User.Identity.Name, model.NewPassword, requireConfirmationToken: false);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }
            string username = OAuthWebSecurity.GetUserName(result.Provider, result.ProviderUserId);
            if (!string.IsNullOrEmpty(username))
            {
                CBFUser user = repo.User.FirstOrDefault(u => u.AppUserName.ToLower() == username.ToLower());
                if (user.ConfirmedEmail)
                {
                    if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    return RedirectToAction("RegisterStepTwo", "Account");
                }
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateSpamPrevention]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                //using (CampusBookFlip.Domain.Abstract.IRepository repo = new CampusBookFlip.Domain.Concrete.EFRepository())
                //{
                CampusBookFlip.Domain.Entities.CBFUser user = repo.User.FirstOrDefault(u => u.AppUserName.ToLower() == model.UserName.ToLower());
                // Check if user already exists
                if (user == null)
                {
                    // Insert name into the profile table
                    string confirmationToken = secure.NewToken;
                    repo.SaveUser(new Domain.Entities.CBFUser { AppUserName = model.UserName.ToLower(), FirstName = model.FirstName, LastName = model.LastName, Paid = false, EmailAddress = model.EmailAddress, ConfirmEmailToken = confirmationToken, ConfirmedEmail = false });
                    OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName.ToLower());
                    //OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);
                    //return RedirectToLocal(returnUrl);
                    var email = new CampusBookFlip.WebUI.Models.ConfirmTokenEmail
                    {
                        ConfirmationToken = confirmationToken,
                        FirstName = model.FirstName,
                        To = model.EmailAddress,
                        From = CampusBookFlip.WebUI.Infrastructure.Constants.EMAIL_NO_REPLY,
                        Subject = CampusBookFlip.WebUI.Infrastructure.Constants.COMPLETE_REGISTRATION_PROCESS,
                    };
                    eservice.Send(email);
                    return RedirectToAction("RegisterStepTwo", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "User name already exists. Please enter a different user name.");
                }
                //}
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(secure.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            repo.Dispose();
        }
    }
}
