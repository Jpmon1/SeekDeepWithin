using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for the register page.
   /// </summary>
   public class RegisterViewModel
   {
      [Required]
      [DataType (DataType.EmailAddress)]
      [Display (Name = "Email Address")]
      [Email(ErrorMessage = "Please supply a valid email address.")]
      public string UserEmail { get; set; }

      [Required]
      [StringLength (100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      [DataType (DataType.Password)]
      [Display (Name = "Password")]
      public string Password { get; set; }

      [DataType (DataType.Password)]
      [Display (Name = "Confirm password")]
      [Compare ("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
   }
}