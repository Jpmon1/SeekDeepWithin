using System.Collections.ObjectModel;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Editing view model for a link.
   /// </summary>
   public class LinkEditViewModel
   {
      private readonly Collection<LinkViewModel> m_Links = new Collection<LinkViewModel> ();

      /// <summary>
      /// Gets or Sets the item id.
      /// </summary>
      public int ItemId { get; set; }

      /// <summary>
      /// Gets or Sets the parent item id.
      /// </summary>
      public int ParentId { get; set; }

      /// <summary>
      /// Gets or Sets the parent item text.
      /// </summary>
      public string ItemText { get; set; }

      /// <summary>
      /// Gets or Sets the type we are editing for.
      /// </summary>
      public string ItemType { get; set; }

      /// <summary>
      /// Gets the id of the previous entry.
      /// </summary>
      public int PreviousEntryId { get; set; }

      /// <summary>
      /// Gets the id of the next entry.
      /// </summary>
      public int NextEntryId { get; set; }

      /// <summary>
      /// Gets or Sets the renedered text.
      /// </summary>
      public string RenderedText { get; set; }

      /// <summary>
      /// Gets the list of links.
      /// </summary>
      public Collection<LinkViewModel> Links { get { return this.m_Links; } }
   }
}