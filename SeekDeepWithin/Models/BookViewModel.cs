using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a book.
   /// </summary>
   public class BookViewModel
   {
      /// <summary>
      /// Initializes a new book view model.
      /// </summary>
      public BookViewModel ()
      {
         this.Versions = new Collection <VersionViewModel> ();
      }

      /// <summary>
      /// Initializes a new book view model.
      /// </summary>
      /// <param name="book">The book to copy data from.</param>
      /// <param name="copyVersions">True to copy version information.</param>
      public BookViewModel (Book book, bool copyVersions = false)
      {
         this.Versions = new Collection<VersionViewModel> ();
         this.Id = book.Id;
         this.Title = book.Title;
         this.Summary = book.Summary;
         if (copyVersions)
         {
            foreach (var version in book.Versions)
               this.Versions.Add (new VersionViewModel (version));
         }
      }

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
      public Collection<VersionViewModel> Versions { get; set; }
   }
}