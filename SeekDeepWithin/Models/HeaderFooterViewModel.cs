using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for headers and footers.
   /// </summary>
   public class HeaderFooterViewModel : IRenderable
   {
      private readonly Collection <LinkViewModel> m_Links = new Collection <LinkViewModel> ();
      private readonly Collection <StyleViewModel> m_Styles = new Collection <StyleViewModel> ();

      /// <summary>
      /// Initializes a new header/footer view model.
      /// </summary>
      public HeaderFooterViewModel ()
      {
         this.Text = string.Empty;
      }

      /// <summary>
      /// Initializes a new header/footer view model.
      /// </summary>
      /// <param name="header">Header data to copy.</param>
      public HeaderFooterViewModel (IHeader header)
      {
         this.HorF = "header";
         this.Id = header.Id;
         this.IsBold = header.IsBold;
         this.IsItalic = header.IsItalic;
         this.Justify = header.Justify;
         this.Text = header.Text;
         foreach (var style in header.StyleList)
            this.Styles.Add(new StyleViewModel(style));
      }

      /// <summary>
      /// Initializes a new header/footer view model.
      /// </summary>
      /// <param name="footer">Footer data to copy.</param>
      public HeaderFooterViewModel (IFooter footer)
      {
         this.HorF = "footer";
         this.Id = footer.Id;
         this.Index = footer.Index;
         this.IsBold = footer.IsBold;
         this.IsItalic = footer.IsItalic;
         this.Justify = footer.Justify;
         this.Text = footer.Text;
         foreach (var link in footer.LinkList)
            this.Links.Add (new LinkViewModel (link));
         foreach (var style in footer.StyleList)
            this.Styles.Add (new StyleViewModel (style));
      }

      /// <summary>
      /// The id of the header or footer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the id of the item this is for.
      /// </summary>
      public int ItemId { get; set; }

      /// <summary>
      /// Gets or Sets the text of the header/footer.
      /// </summary>
      [Required]
      public string Text { get; set; }

      /// <summary>
      /// Gets the list of links for this renderable.
      /// </summary>
      public Collection <LinkViewModel> Links
      {
         get { return m_Links; }
      }

      /// <summary>
      /// Get or Sets the styles for this renderable.
      /// </summary>
      public Collection <StyleViewModel> Styles
      {
         get { return m_Styles; }
      }

      /// <summary>
      /// Get or Sets the footers for this passage.
      /// </summary>
      public Collection <HeaderFooterViewModel> Footers
      {
         get { return new Collection <HeaderFooterViewModel> (); }
      }

      /// <summary>
      /// Gets or Sets the index for a footer.
      /// </summary>
      public int Index { get; set; }

      /// <summary>
      /// Gets or Sets if the content should be bolded.
      /// </summary>
      public bool IsBold { get; set; }

      /// <summary>
      /// Gets or Sets if the content should be italicized.
      /// </summary>
      public bool IsItalic { get; set; }

      /// <summary>
      /// Gets or Sets the justification of the header.
      /// </summary>
      public int Justify { get; set; }

      /// <summary>
      /// Gets or Sets the type either header/footer.
      /// </summary>
      public string HorF { get; set; }

      /// <summary>
      /// Gets or Sets the number for footers.
      /// </summary>
      public int Number { get; set; }

      /// <summary>
      /// Gets what this header/footer is for.
      /// </summary>
      public string ItemType { get; set; }

      /// <summary>
      /// Gets or Sets the item's text.
      /// </summary>
      public string ItemText { get; set; }

      /// <summary>
      /// Gets or Sets the renderer.
      /// </summary>
      public SdwRenderer Renderer { get; set; }

      /// <summary>
      /// Renders the passage.
      /// </summary>
      /// <returns>The html to display for the passage.</returns>
      public string Render ()
      {
         if (this.Renderer == null)
            this.Renderer = new SdwRenderer ();
         return Renderer.Render (this);
      }
   }
}