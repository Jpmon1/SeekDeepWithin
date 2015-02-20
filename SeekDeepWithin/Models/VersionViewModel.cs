using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a version.
   /// </summary>
   public class VersionViewModel
   {
      /// <summary>
      /// Initializes a new version view model.
      /// </summary>
      public VersionViewModel ()
      {
         this.SubBooks = new Collection <SubBookViewModel> ();
      }

      /// <summary>
      /// Initializes a new version view model.
      /// </summary>
      /// <param name="version">The version to copy data from.</param>
      public VersionViewModel (Version version)
      {
         var source = version.VersionSources.FirstOrDefault ();
         this.SubBooks = new Collection<SubBookViewModel> ();
         this.Id = version.Id;
         this.Title = version.Title;
         this.BookId = version.Book.Id;
         this.Abbreviation = version.Abbreviation;
         this.PublishDate = version.PublishDate;
         this.Contents = version.Contents;
         this.Book = new BookViewModel (version.Book);
         this.DefaultReadChapter = version.DefaultReadChapter;
         this.SourceName = source == null ? string.Empty : source.Source.Name;
         this.SourceUrl = source == null ? string.Empty : source.Source.Url;
      }

      /// <summary>
      /// Gets the id of the table.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the contents of this version.
      /// </summary>
      public string Contents { get; set; }

      /// <summary>
      /// Gets or Sets an abbreviation to use for this version.
      /// </summary>
      public string Abbreviation { get; set; }

      /// <summary>
      /// Gets or Sets the book id.
      /// </summary>
      public int BookId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the table.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or Sets the date the version was published.
      /// </summary>
      [Display (Name = "Published Date")]
      public string PublishDate { get; set; }

      /// <summary>
      /// Gets or Sets the source name.
      /// </summary>
      public string SourceName { get; set; }

      /// <summary>
      /// Gets or Sets the source url.
      /// </summary>
      public string SourceUrl { get; set; }

      /// <summary>
      /// Gets or Sets the chapter id to read when opeing a version.
      /// </summary>
      public int DefaultReadChapter { get; set; }

      /// <summary>
      /// Gets or Sets the book of the version
      /// </summary>
      public BookViewModel Book { get; set; }

      /// <summary>
      /// Gets or Sets the list of sub books.
      /// </summary>
      public Collection<SubBookViewModel> SubBooks { get; set; }
   }
}