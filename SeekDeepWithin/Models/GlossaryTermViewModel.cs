using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a glossary term.
   /// </summary>
   public class GlossaryTermViewModel
   {
      private readonly Collection<TagViewModel> m_Tags;
      private readonly Collection<LinkViewModel> m_SeeAlsos;
      private readonly Collection <GlossaryItemViewModel> m_Items;

      /// <summary>
      /// Initializes a new glossary term view model.
      /// </summary>
      public GlossaryTermViewModel ()
      {
         this.m_Tags = new Collection<TagViewModel> ();
         this.m_SeeAlsos = new Collection <LinkViewModel> ();
         this.m_Items = new Collection <GlossaryItemViewModel> ();
      }

      public GlossaryTermViewModel (GlossaryTerm term)
      {
         this.Id = term.Id;
         this.Name = term.Name;
         var renderer = new SdwRenderer ();
         this.m_Tags = new Collection<TagViewModel> ();
         this.m_SeeAlsos = new Collection<LinkViewModel> ();
         this.m_Items = new Collection<GlossaryItemViewModel> ();
         foreach (var termTag in term.Tags)
            this.Tags.Add (new TagViewModel { ItemId = termTag.Id, Name = termTag.Tag.Name, Id = termTag.Tag.Id });
         foreach (var item in term.Items)
            this.Items.Add (new GlossaryItemViewModel (item, renderer) { Term = this });
         foreach (var seeAlso in term.SeeAlsos)
            this.SeeAlsos.Add (new LinkViewModel { Url = seeAlso.Link.Url, Name = seeAlso.Name });
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
      /// Gets the list of tags for this term.
      /// </summary>
      public Collection<TagViewModel> Tags { get { return this.m_Tags; } }

      /// <summary>
      /// Gets the list of see also's for this term.
      /// </summary>
      public Collection<LinkViewModel> SeeAlsos { get { return this.m_SeeAlsos; } }

      /// <summary>
      /// Get or Sets the list of entries.
      /// </summary>
      public Collection<GlossaryItemViewModel> Items { get { return this.m_Items; } }
   }
}