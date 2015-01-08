using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for headers and footers.
   /// </summary>
   public class HeaderFooterViewModel
   {
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
      /// Gets or Sets the header type.
      /// </summary>
      public string Type { get; set; }
   }
}