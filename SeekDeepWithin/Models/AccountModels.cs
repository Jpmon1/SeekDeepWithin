﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SeekDeepWithin.Models
{
   public class RegisterExternalLoginModel
   {
      [Required]
      [Display (Name = "User name")]
      public string UserName { get; set; }

      public string ExternalLoginData { get; set; }
   }

   public class ExternalLogin
   {
      public string Provider { get; set; }
      public string ProviderDisplayName { get; set; }
      public string ProviderUserId { get; set; }
   }

   public class ManageModel
   {
      [Required]
      [DataType (DataType.Password)]
      [Display (Name = "Current password")]
      public string OldPassword { get; set; }

      [Required]
      [StringLength (100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      [DataType (DataType.Password)]
      [Display (Name = "New password")]
      public string NewPassword { get; set; }

      [DataType (DataType.Password)]
      [Display (Name = "Confirm new password")]
      [Compare ("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }

      public bool LoadOnScroll { get; set; }
   }
}
