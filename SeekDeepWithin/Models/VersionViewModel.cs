using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a version.
   /// </summary>
   public class VersionViewModel
   {
      private readonly Collection<SubBookViewModel> m_SubBooks = new Collection<SubBookViewModel> ();

      /// <summary>
      /// Initializes a new version view model.
      /// </summary>
      public VersionViewModel () { }

      /// <summary>
      /// Initializes a new version view model.
      /// </summary>
      /// <param name="version">The version to copy data from.</param>
      /// <param name="copySubBooks">True to copy sub book information, default is false.</param>
      public VersionViewModel (Version version, bool copySubBooks = false)
      {
         this.Id = version.Id;
         this.Title = version.Title;
         this.BookId = version.Book.Id;
         this.SourceUrl = version.SourceUrl;
         this.SourceName = version.SourceName;
         this.PublishDate = version.PublishDate;
         this.Contents = version.Contents ?? string.Empty;
         this.Book = new BookViewModel (version.Book);
         this.DefaultReadChapter = version.DefaultReadChapter;
         if (version.Term != null)
            this.Term = new TermViewModel (version.Term);
         if (copySubBooks)
         {
            foreach (var subBook in version.SubBooks)
               this.SubBooks.Add (new SubBookViewModel (subBook, false, this));
         }
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
      /// Gets or Sets the associated term.
      /// </summary>
      public TermViewModel Term { get; set; }

      /// <summary>
      /// Gets or Sets the list of sub books.
      /// </summary>
      public Collection<SubBookViewModel> SubBooks { get { return this.m_SubBooks; } }
   }
}