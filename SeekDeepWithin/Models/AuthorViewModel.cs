using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for writers.
   /// </summary>
   public class AuthorViewModel
   {
      /// <summary>
      /// Initializes a new author view model.
      /// </summary>
      public AuthorViewModel ()
      {
         this.Written = new Collection<SubBookViewModel> ();
      }

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
      public Collection<SubBookViewModel> Written { get; set; }
   }
}