﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Filters;
using SeekDeepWithin.Pocos;
using WebMatrix.WebData;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for account actions.
   /// </summary>
   [Authorize]
   [InitializeSimpleMembership]
   public class AccountController : Controller
   {
      private readonly UsersContext m_Db = new UsersContext ();

      /// <summary>
      /// Gets the login view.
      /// </summary>
      /// <param name="returnUrl">The url to return to after login.</param>
      /// <returns>The login view.</returns>
      [AllowAnonymous]
      public ActionResult Login (string returnUrl)
      {
         ViewBag.ReturnUrl = returnUrl;
         return View ();
      }

      /// <summary>
      /// Gets the login view.
      /// </summary>
      /// <returns>The login view.</returns>
      [AllowAnonymous]
      public ActionResult Token ()
      {
         return PartialView ();
      }

      /// <summary>
      /// Attempts to login.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [AllowAnonymous]
      public ActionResult LoginRequest (LoginViewModel viewModel)
      {
         if (!ModelState.IsValid)
            return Json (new {status="fail", message = "Invalid login information."});
         if (WebSecurity.Login (viewModel.UserEmail, viewModel.Password, false))
            return Json (new {status="success", message = "Login successful"});
         return Json (new {status="fail", message = "Login information incorrect."});
      }

      /// <summary>
      /// Logs a user in.
      /// </summary>
      /// <param name="viewModel">Login information.</param>
      /// <param name="returnUrl">The url to return to after login.</param>
      /// <returns>The return url view, index or error page.</returns>
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public ActionResult Login (LoginViewModel viewModel, string returnUrl)
      {
         if (ModelState.IsValid && WebSecurity.Login (viewModel.UserEmail, viewModel.Password, viewModel.RememberMe))
         {
            if (!string.IsNullOrWhiteSpace(returnUrl))
               return Redirect (returnUrl);
            return RedirectToAction ("Index", "Home");
         }

         // If we got this far, something failed, redisplay form
         ModelState.AddModelError ("", "The user name or password provided is incorrect.");
         return View (viewModel);
      }

      /// <summary>
      /// Logs the current user out of the system.
      /// </summary>
      /// <returns>The reffered url, or the index.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult LogOff ()
      {
         WebSecurity.Logout ();
         return RedirectToAction ("Index", "Home");
      }

      /// <summary>
      /// Gets the register view.
      /// </summary>
      /// <returns>The register view.</returns>
      [AllowAnonymous]
      public ActionResult Register ()
      {
         return View ();
      }

      /// <summary>
      /// Registers a new user.
      /// </summary>
      /// <param name="viewModel">User information.</param>
      /// <returns>Index or view with errors.</returns>
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public ActionResult Register (RegisterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            // Attempt to register the user
            try
            {
               WebSecurity.CreateUserAndAccount (viewModel.UserEmail, viewModel.Password);
               WebSecurity.Login (viewModel.UserEmail, viewModel.Password);
               var user = this.m_Db.UserProfiles.Find (WebSecurity.GetUserId (viewModel.UserEmail));
               if (user != null)
               {
                  // Create new user data with default options...
                  user.UserData = new UserData
                  {
                     ShowEditActions = false,
                     UserProfileId = user.UserId
                  };
                  this.m_Db.SaveChanges ();
               }
               return RedirectToAction ("Index", "Home");
            }
            catch (MembershipCreateUserException e)
            {
               ModelState.AddModelError ("", ErrorCodeToString (e.StatusCode));
            }
         }

         // If we got this far, something failed, redisplay form
         return View (viewModel);
      }

      //
      // POST: /Account/Disassociate

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Disassociate (string provider, string providerUserId)
      {
         string ownerAccount = OAuthWebSecurity.GetUserName (provider, providerUserId);
         ManageMessageId? message = null;

         // Only disassociate the account if the currently logged in user is the owner
         if (ownerAccount == User.Identity.Name)
         {
            // Use a transaction to prevent the user from deleting their last login credential
            using (var scope = new TransactionScope (TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {
               bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount (WebSecurity.GetUserId (User.Identity.Name));
               if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName (User.Identity.Name).Count > 1)
               {
                  OAuthWebSecurity.DeleteAccount (provider, providerUserId);
                  scope.Complete ();
                  message = ManageMessageId.RemoveLoginSuccess;
               }
            }
         }

         return RedirectToAction ("Manage", new { Message = message });
      }

      //
      // GET: /Account/Manage

      public ActionResult Manage (ManageMessageId? message)
      {
         ViewBag.StatusMessage =
             message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
             : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
             : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
             : "";
         var userId = WebSecurity.GetUserId (User.Identity.Name);
         bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount (userId);
         ViewBag.HasLocalPassword = hasLocalAccount;
         ViewBag.ReturnUrl = Url.Action ("Manage");
         var model = new ManageModel ();
         if (hasLocalAccount) {
            var user = this.m_Db.UserProfiles.Find (userId);
            model.LoadOnScroll = user.UserData.LoadOnScroll ?? true;
         }
         return View (model);
      }

      [HttpPost]
      public ActionResult SaveSettings (ManageModel model)
      {
         var userId = WebSecurity.GetUserId (User.Identity.Name);
         var user = this.m_Db.UserProfiles.Find (userId);
         user.UserData.LoadOnScroll = model.LoadOnScroll;
         this.m_Db.SaveChanges ();
         return Json (new { status = "success", messaeg = "Changes saved." }, JsonRequestBehavior.AllowGet);
      }

      //
      // POST: /Account/Manage

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Manage (ManageModel model)
      {
         var userId = WebSecurity.GetUserId (User.Identity.Name);
         bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount (userId);
         ViewBag.HasLocalPassword = hasLocalAccount;
         ViewBag.ReturnUrl = Url.Action ("Manage");
         if (hasLocalAccount)
         {
            if (ModelState.IsValid) {
               // ChangePassword will throw an exception rather than return false in certain failure scenarios.
               bool changePasswordSucceeded;
               try
               {
                  changePasswordSucceeded = WebSecurity.ChangePassword (User.Identity.Name, model.OldPassword, model.NewPassword);
               }
               catch (Exception)
               {
                  changePasswordSucceeded = false;
               }

               if (changePasswordSucceeded)
               {
                  return RedirectToAction ("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
               }
               ModelState.AddModelError ("", "The current password is incorrect or the new password is invalid.");
            }
         }
         else
         {
            // User does not have a local password so remove any validation errors caused by a missing
            // OldPassword field
            ModelState state = ModelState["OldPassword"];
            if (state != null)
            {
               state.Errors.Clear ();
            }

            if (ModelState.IsValid)
            {
               try
               {
                  WebSecurity.CreateAccount (User.Identity.Name, model.NewPassword);
                  return RedirectToAction ("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
               }
               catch (Exception)
               {
                  ModelState.AddModelError ("", String.Format ("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
               }
            }
         }

         // If we got this far, something failed, redisplay form
         return View (model);
      }

      //
      // POST: /Account/ExternalLogin

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public ActionResult ExternalLogin (string provider, string returnUrl)
      {
         return new ExternalLoginResult (provider, Url.Action ("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
      }

      //
      // GET: /Account/ExternalLoginCallback

      [AllowAnonymous]
      public ActionResult ExternalLoginCallback (string returnUrl)
      {
         AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication (Url.Action ("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
         if (!result.IsSuccessful)
         {
            return RedirectToAction ("ExternalLoginFailure");
         }

         if (OAuthWebSecurity.Login (result.Provider, result.ProviderUserId, createPersistentCookie: false))
         {
            return RedirectToLocal (returnUrl);
         }

         if (User.Identity.IsAuthenticated)
         {
            // If the current user is logged in add the new account
            OAuthWebSecurity.CreateOrUpdateAccount (result.Provider, result.ProviderUserId, User.Identity.Name);
            return RedirectToLocal (returnUrl);
         }

         // User is new, ask for their desired membership name
         string loginData = OAuthWebSecurity.SerializeProviderUserId (result.Provider, result.ProviderUserId);
         ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData (result.Provider).DisplayName;
         ViewBag.ReturnUrl = returnUrl;
         return View ("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
      }

      //
      // POST: /Account/ExternalLoginConfirmation

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public ActionResult ExternalLoginConfirmation (RegisterExternalLoginModel model, string returnUrl)
      {
         string provider;
         string providerUserId;

         if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId (model.ExternalLoginData, out provider, out providerUserId))
         {
            return RedirectToAction ("Manage");
         }

         if (ModelState.IsValid)
         {
            // Insert a new user into the database
            using (var db = new UsersContext ())
            {
               UserProfile user = db.UserProfiles.FirstOrDefault (u => String.Equals (u.Email, model.UserName, StringComparison.CurrentCultureIgnoreCase));
               // Check if user already exists
               if (user == null)
               {
                  // Insert name into the profile table
                  db.UserProfiles.Add (new UserProfile { Email = model.UserName });
                  db.SaveChanges ();

                  OAuthWebSecurity.CreateOrUpdateAccount (provider, providerUserId, model.UserName);
                  OAuthWebSecurity.Login (provider, providerUserId, createPersistentCookie: false);

                  return RedirectToLocal (returnUrl);
               }
               ModelState.AddModelError ("UserName", "User name already exists. Please enter a different user name.");
            }
         }

         ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData (provider).DisplayName;
         ViewBag.ReturnUrl = returnUrl;
         return View (model);
      }

      //
      // GET: /Account/ExternalLoginFailure

      [AllowAnonymous]
      public ActionResult ExternalLoginFailure ()
      {
         return View ();
      }

      [AllowAnonymous]
      [ChildActionOnly]
      public ActionResult ExternalLoginsList (string returnUrl)
      {
         ViewBag.ReturnUrl = returnUrl;
         return PartialView ("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
      }

      [ChildActionOnly]
      public ActionResult RemoveExternalLogins ()
      {
         ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName (User.Identity.Name);
         var externalLogins = new List<ExternalLogin> ();
         foreach (OAuthAccount account in accounts)
         {
            AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData (account.Provider);

            externalLogins.Add (new ExternalLogin
            {
               Provider = account.Provider,
               ProviderDisplayName = clientData.DisplayName,
               ProviderUserId = account.ProviderUserId,
            });
         }

         ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount (WebSecurity.GetUserId (User.Identity.Name));
         return PartialView ("_RemoveExternalLoginsPartial", externalLogins);
      }

      #region Helpers
      private ActionResult RedirectToLocal (string returnUrl)
      {
         if (Url.IsLocalUrl (returnUrl))
         {
            return Redirect (returnUrl);
         }
         else
         {
            return RedirectToAction ("Index", "Home");
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
         public ExternalLoginResult (string provider, string returnUrl)
         {
            Provider = provider;
            ReturnUrl = returnUrl;
         }

         public string Provider { get; private set; }
         public string ReturnUrl { get; private set; }

         public override void ExecuteResult (ControllerContext context)
         {
            OAuthWebSecurity.RequestAuthentication (Provider, ReturnUrl);
         }
      }

      private static string ErrorCodeToString (MembershipCreateStatus createStatus)
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
   }
}
