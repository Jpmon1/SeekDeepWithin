using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeekDeepWithin.Pocos
{
   [Table ("UserProfile")]
   public class UserProfile
   {
      /// <summary>
      /// Gets the id of the user.
      /// </summary>
      [Key]
      [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
      public int UserId { get; set; }

      /// <summary>
      /// Gets the first name of the user.
      /// </summary>
      public string Alias { get; set; }

      /// <summary>
      /// Gets the email of the user.
      /// </summary>
      [DataType (DataType.EmailAddress)]
      public string Email { get; set; }

      /// <summary>
      /// Get or Sets the user options
      /// </summary>
      public virtual UserData UserData { get; set; }
   }
}