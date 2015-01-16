using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SeekDeepWithin.Domain;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for headers and footers.
   /// </summary>
   public class HeaderFooterViewModel
   {
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
         this.Type = "header";
         this.Id = header.Id;
         this.IsBold = header.IsBold;
         this.IsItalic = header.IsItalic;
         this.Justify = header.Justify;
         this.Text = header.Text;
      }

      /// <summary>
      /// Initializes a new header/footer view model.
      /// </summary>
      /// <param name="footer">Footer data to copy.</param>
      public HeaderFooterViewModel (IFooter footer)
      {
         this.Type = "footer";
         this.Id = footer.Id;
         this.Index = footer.Index;
         this.IsBold = footer.IsBold;
         this.IsItalic = footer.IsItalic;
         this.Justify = footer.Justify;
         this.Text = footer.Text;
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
      [AllowHtml]
      public string Text { get; set; }

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
      public string Type { get; set; }

      /// <summary>
      /// Gets or Sets the number for footers.
      /// </summary>
      public int Number { get; set; }

      /// <summary>
      /// Gets what this header/footer is for.
      /// </summary>
      public string For { get; set; }
   }
}