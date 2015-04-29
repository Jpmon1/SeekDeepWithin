using System.Collections.ObjectModel;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a sub book.
   /// </summary>
   public class SubBookViewModel
   {
      private readonly Collection<AbbreviationViewModel> m_Abbreviations = new Collection <AbbreviationViewModel> ();
      private readonly Collection<ChapterViewModel> m_Chapters = new Collection <ChapterViewModel> ();
      private readonly Collection<WriterViewModel> m_Writers = new Collection<WriterViewModel> ();
      private readonly Collection<TagViewModel> m_Tags = new Collection <TagViewModel> ();

      /// <summary>
      /// Initializes a new sub book view model.
      /// </summary>
      public SubBookViewModel () {}

      /// <summary>
      /// Initializes a new sub book view model.
      /// </summary>
      /// <param name="subBook">The sub book to copy data from.</param>
      public SubBookViewModel (VersionSubBook subBook)
      {
         this.Id = subBook.Id;
         this.Hide = subBook.Hide;
         this.Alias = subBook.Alias;
         this.Name = subBook.SubBook.Name;
         this.SubBookId = subBook.SubBook.Id;
         this.VersionId = subBook.Version.Id;
         this.BookId = subBook.SubBook.Book.Id;
         this.Version = new VersionViewModel (subBook.Version);
         foreach (var abbreviation in subBook.SubBook.Abbreviations)
            this.m_Abbreviations.Add (new AbbreviationViewModel { Id = abbreviation.Id, Text = abbreviation.Text });
         foreach (var subBookTag in subBook.SubBook.Tags)
            this.Tags.Add (new TagViewModel { ItemId = subBookTag.Id, Name = subBookTag.Tag.Name, Id = subBookTag.Tag.Id });

         foreach (var writer in subBook.SubBook.Writers)
         {
            this.Writers.Add (new WriterViewModel
            {
               IsTranslator = writer.IsTranslator,
               Id = writer.Writer.Id,
               Name = writer.Writer.Name
            });
         }
      }

      /// <summary>
      /// Gets or Sets the alias of the sub book.
      /// </summary>
      public string Alias { get; set; }

      /// <summary>
      /// Gets or Sets the id of this sub book.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the id of the book this sub book belongs to.
      /// </summary>
      public int SubBookId { get; set; }

      /// <summary>
      /// Gets or Sets the id of the book this sub book belongs to.
      /// </summary>
      public int BookId { get; set; }

      /// <summary>
      /// Gets or Sets the id of the version this sub book belongs to.
      /// </summary>
      public int VersionId { get; set; }

      /// <summary>
      /// Gets or Sets the name of this sub book.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets if this is hidden.
      /// </summary>
      public bool Hide { get; set; }

      /// <summary>
      /// Gets the about information.
      /// </summary>
      public string About { get; set; }

      /// <summary>
      /// Gets or Sets the version of this sub book.
      /// </summary>
      public VersionViewModel Version { get; set; }

      /// <summary>
      /// Gets the list of tags for this term.
      /// </summary>
      public Collection<TagViewModel> Tags { get { return this.m_Tags; } }

      /// <summary>
      /// Gets or Sets the list of authors.
      /// </summary>
      public Collection<AbbreviationViewModel> Abbreviations { get { return this.m_Abbreviations; } }

      /// <summary>
      /// Gets or Sets the list of writers.
      /// </summary>
      public Collection<WriterViewModel> Writers { get { return this.m_Writers; } }

      /// <summary>
      /// Gets or Sets the list of chapters.
      /// </summary>
      public Collection<ChapterViewModel> Chapters { get { return this.m_Chapters; } }
   }
}