using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SeekDeepWithin.Models
{
   public class GlossaryTermViewModel
   {
      /// <summary>
      /// Initializes a new glossary term view model.
      /// </summary>
      public GlossaryTermViewModel ()
      {
         this.Entries = new Collection <GlossaryEntryViewModel> ();
      }

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
      public Collection<GlossaryEntryViewModel> Entries { get; set; }
   }
}