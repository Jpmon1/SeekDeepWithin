using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SeekDeepWithin.Models
{
   public class AuthorViewModel
   {
      /// <summary>
      /// Gets or Sets the id of the author.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of the author.
      /// </summary>
      [Required]
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets information about this author.
      /// </summary>
      [AllowHtml]
      public string About { get; set; }

      /// <summary>
      /// Gets or Sets the list of version this author has written.
      /// </summary>
      public ICollection<VersionViewModel> Written { get; set; }
   }
}