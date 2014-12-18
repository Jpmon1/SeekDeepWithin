using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SeekDeepWithin.Models
{
   public class GlossaryEntryViewModel
   {
      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text of this entry.
      /// </summary>
      [Required]
      [AllowHtml]
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the id of the term this entry belongs to.
      /// </summary>
      public int TermId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the term.
      /// </summary>
      public string TermName { get; set; }

      /// <summary>
      /// Gets or Sets the source name.
      /// </summary>
      public string SourceName { get; set; }

      /// <summary>
      /// Gets or Sets the source url.
      /// </summary>
      public string SourceUrl { get; set; }
   }
}