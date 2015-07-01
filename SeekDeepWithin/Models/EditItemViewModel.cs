using System.Collections.ObjectModel;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents an item that is being edited.
   /// </summary>
   public class EditItemViewModel
   {
      private readonly Collection <LinkViewModel> m_Links = new Collection <LinkViewModel> ();
      private readonly Collection <StyleViewModel> m_Styles = new Collection <StyleViewModel> ();
      private readonly Collection <HeaderFooterViewModel> m_Footers = new Collection <HeaderFooterViewModel> ();

      /// <summary>
      /// Initializes a new edit item view model.
      /// </summary>
      public EditItemViewModel (int id, EditItemType type)
      {
         this.ItemId = id;
         this.HasLinks = true;
         this.HasFooters = true;
         this.Text = string.Empty;
         this.Html = string.Empty;
         this.EditItemType = type;
      }

      /// <summary>
      /// Gets the id of the item we are editing.
      /// </summary>
      public int ItemId { get; private set; }

      /// <summary>
      /// Gets the type of item we are editing.
      /// </summary>
      public EditItemType EditItemType { get; private set; }

      /// <summary>
      /// Gets or Sets if this item has links or not.
      /// </summary>
      public bool HasLinks { get; set; }

      /// <summary>
      /// Gets or Sets if this item has footers or not.
      /// </summary>
      public bool HasFooters { get; set; }

      /// <summary>
      /// Gets or Sets the text of the item.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the text of the item with the rendered HTML included.
      /// </summary>
      public string Html { get; set; }

      /// <summary>
      /// Gets the footer id if needed.
      /// </summary>
      public int FooterId { get; set; }

      /// <summary>
      /// Gets the collection of links.
      /// </summary>
      public Collection<LinkViewModel> Links { get { return this.m_Links; } }

      /// <summary>
      /// Gets the collection of styles.
      /// </summary>
      public Collection<StyleViewModel> Styles { get { return this.m_Styles; } }

      /// <summary>
      /// Gets the collection of footers.
      /// </summary>
      public Collection<HeaderFooterViewModel> Footers { get { return this.m_Footers; } }
   }
}