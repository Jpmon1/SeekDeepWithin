using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SeekDeepWithin.Models
{
   public class BookViewModel
   {

      /// <summary>
      /// Gets or Sets the id of the book.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the title of the book.
      /// </summary>
      [Required]
      public string Title { get; set; }

      /// <summary>
      /// Gets or Sets a brief summary of the book.
      /// </summary>
      [AllowHtml]
      public string Summary { get; set; }

      /// <summary>
      /// Gets or Sets the list of versions.
      /// </summary>
      public ICollection<VersionViewModel> Versions { get; set; }
   }
}