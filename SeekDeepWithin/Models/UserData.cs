using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeekDeepWithin.Models
{
   public class UserData
   {
      [Key]
      [ForeignKey ("UserProfile")]
      public int UserProfileId { get; set; }

      /// <summary>
      /// Gets or Sets if we should show the edit actions for the user, if he/she is an editor.
      /// </summary>
      public bool? ShowEditActions { get; set; }

      /// <summary>
      /// Get or Sets the associated user profile.
      /// </summary>
      public virtual UserProfile UserProfile { get; set; }
   }
}