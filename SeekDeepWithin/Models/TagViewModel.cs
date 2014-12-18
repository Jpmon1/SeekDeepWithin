using System.ComponentModel.DataAnnotations;

namespace SeekDeepWithin.Models
{
   public class TagViewModel
   {
      /// <summary>
      /// Gets or Sets the id of the tag.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Get or Sets name of this tag.
      /// </summary>
      [Required]
      public string Name { get; set; }
   }
}