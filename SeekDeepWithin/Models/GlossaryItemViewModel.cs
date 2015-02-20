using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   public class GlossaryItemViewModel
   {
      private readonly Collection <GlossaryEntryViewModel> m_Entries;

      /// <summary>
      /// Initializes a new glossary item.
      /// </summary>
      public GlossaryItemViewModel ()
      {
         this.m_Entries = new Collection <GlossaryEntryViewModel> ();
      }

      public GlossaryItemViewModel (GlossaryItem item, SdwRenderer renderer)
      {
         this.Id = item.Id;
         var source = item.Sources.FirstOrDefault ();
         this.m_Entries = new Collection<GlossaryEntryViewModel> ();
         if (source != null)
         {
            this.SourceName = source.Source.Name;
            this.SourceUrl = source.Source.Url;
         }
         foreach (var entry in item.Entries)
            this.Entries.Add(new GlossaryEntryViewModel (entry, renderer));
      }

      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the id of the term this entry belongs to.
      /// </summary>
      public GlossaryTermViewModel Term { get; set; }

      /// <summary>
      /// Gets or Sets the source name.
      /// </summary>
      [Required]
      public string SourceName { get; set; }

      /// <summary>
      /// Gets or Sets the source url.
      /// </summary>
      [Required]
      public string SourceUrl { get; set; }

      /// <summary>
      /// Gets the list of entries.
      /// </summary>
      public Collection<GlossaryEntryViewModel> Entries { get { return this.m_Entries; } }
   }
}