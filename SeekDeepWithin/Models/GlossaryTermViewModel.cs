using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   public class GlossaryTermViewModel
   {
      private readonly Collection <GlossaryItemViewModel> m_Items;

      /// <summary>
      /// Initializes a new glossary term view model.
      /// </summary>
      public GlossaryTermViewModel ()
      {
         this.m_Items = new Collection <GlossaryItemViewModel> ();
      }

      public GlossaryTermViewModel (GlossaryTerm term)
      {
         this.Id = term.Id;
         this.Name = term.Name;
         var renderer = new SdwRenderer ();
         this.m_Items = new Collection <GlossaryItemViewModel> ();
         foreach (var item in term.Items)
            this.Items.Add(new GlossaryItemViewModel (item, renderer) { Term = this });
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
      public Collection<GlossaryItemViewModel> Items { get { return this.m_Items; } }
   }
}