using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for the login page.
   /// </summary>
   public class LoginViewModel
   {
      [Email]
      [Required]
      [Display (Name = "Email Address")]
      [DataType (DataType.EmailAddress)]
      public string UserEmail { get; set; }

      [Required]
      [DataType (DataType.Password)]
      [Display (Name = "Password")]
      public string Password { get; set; }

      [Display (Name = "Remember me?")]
      public bool RememberMe { get; set; }
   }
}