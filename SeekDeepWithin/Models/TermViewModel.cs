using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a glossary term.
   /// </summary>
   public class TermViewModel
   {
      private readonly Collection<TermItemViewModel> m_Items = new Collection <TermItemViewModel> ();
      private readonly Collection<TermLinkViewModel> m_Links = new Collection <TermLinkViewModel> ();

      /// <summary>
      /// Initializes a new term view model from the given model.
      /// </summary>
      public TermViewModel () { }

      /// <summary>
      /// Initializes a new term view model from the given model.
      /// </summary>
      /// <param name="term">The model to copy data from.</param>
      public TermViewModel (Term term)
      {
         this.Id = term.Id;
         this.Name = term.Name;
         var renderer = new SdwRenderer ();
         foreach (var link in term.Links)
            this.Links.Add(new TermLinkViewModel (link));
         foreach (var item in term.Items)
            this.Items.Add (new TermItemViewModel (item, renderer) { Term = this });
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
      /// Gets or Sets the referenced book.
      /// </summary>
      public BookViewModel Book { get; set; }

      /// <summary>
      /// Gets the list of links for this term.
      /// </summary>
      public Collection<TermLinkViewModel> Links { get { return this.m_Links; } }

      /// <summary>
      /// Get or Sets the list of entries.
      /// </summary>
      public Collection<TermItemViewModel> Items { get { return this.m_Items; } }
   }
}