using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SeekDeepWithin.Models
{
   public class GlossaryTermViewModel
   {
      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Get or Sets name of this item.
      /// </summary>
      [Required]
      public string Name { get; set; }

      /// <summary>
      /// Get or Sets the list of entries.
      /// </summary>
      public virtual ICollection<GlossaryEntryViewModel> Entries { get; set; }
   }
}