using System.Collections.ObjectModel;
using SeekDeepWithin.Pocos;

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
      /// Initializes a new sub book view model.
      /// </summary>
      /// <param name="subBook">The sub book to copy data from.</param>
      public SubBookViewModel (SubBook subBook)
      {
         this.Chapters = new Collection<ChapterViewModel> ();
         this.Writers = new Collection<WriterViewModel> ();
         this.Id = subBook.Id;
         this.Name = subBook.Name;
         this.VersionId = subBook.Version.Id;
         this.Version = new VersionViewModel (subBook.Version);

         foreach (var writer in subBook.Writers)
         {
            this.Writers.Add (new WriterViewModel
            {
               IsTranslator = writer.IsTranslator,
               Id = writer.Author.Id,
               Name = writer.Author.Name
            });
         }
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