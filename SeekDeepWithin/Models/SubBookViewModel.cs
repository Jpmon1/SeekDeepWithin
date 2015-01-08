using System.Collections.ObjectModel;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a sub book.
   /// </summary>
   public class SubBookViewModel
   {
      /// <summary>
      /// Initializes a new sub book view model.
      /// </summary>
      public SubBookViewModel ()
      {
         this.Chapters = new Collection <ChapterViewModel> ();
         this.Writers = new Collection <WriterViewModel> ();
      }

      /// <summary>
      /// Gets or Sets the id of this sub book.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the id of the version this sub book belongs to.
      /// </summary>
      public int VersionId { get; set; }

      /// <summary>
      /// Gets or Sets the order of this sub book.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the name of this sub book.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the version of this sub book.
      /// </summary>
      public VersionViewModel Version { get; set; }

      /// <summary>
      /// Gets or Sets the list of authors.
      /// </summary>
      public Collection<WriterViewModel> Writers { get; set; }

      /// <summary>
      /// Gets or Sets the list of chapters.
      /// </summary>
      public Collection<ChapterViewModel> Chapters { get; set; }
   }
}