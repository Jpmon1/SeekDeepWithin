using System.Collections.Generic;
using System.Collections.ObjectModel;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a sub book.
   /// </summary>
   public class SubBookViewModel
   {
      private readonly Collection<ChapterViewModel> m_Chapters = new Collection <ChapterViewModel> ();
      private readonly List<string> m_Abbrevations = new List<string> ();

      /// <summary>
      /// Initializes a new sub book view model.
      /// </summary>
      public SubBookViewModel () {}

      /// <summary>
      /// Initializes a new sub book view model.
      /// </summary>
      /// <param name="subBook">The sub book to copy data from.</param>
      /// <param name="copyChapters">True to copy the chapters.</param>
      /// <param name="version">The version of the sub book.</param>
      public SubBookViewModel (VersionSubBook subBook, bool copyChapters = false, VersionViewModel version = null)
      {
         this.Id = subBook.Id;
         this.Hide = subBook.Hide;
         this.Alias = subBook.Alias;
         this.Name = subBook.Term.Name;
         this.Term = new TermViewModel (subBook.Term);
         this.Version = version ?? new VersionViewModel (subBook.Version);
         if (copyChapters)
         {
            foreach (var chapter in subBook.Chapters)
               this.Chapters.Add(new ChapterViewModel(chapter, this));
         }
      }

      /// <summary>
      /// Gets the associated term.
      /// </summary>
      public TermViewModel Term { get; private set; }

      /// <summary>
      /// Gets or Sets the alias of the sub book.
      /// </summary>
      public string Alias { get; set; }

      /// <summary>
      /// Gets or Sets the id of this sub book.
      /// </summary>
      public int Id { get; set; }

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
      /// Gets the list of abbreviations for this sub book.
      /// </summary>
      public List<string> Abbreviations { get { return this.m_Abbrevations; } }

      /// <summary>
      /// Gets or Sets the list of chapters.
      /// </summary>
      public Collection<ChapterViewModel> Chapters { get { return this.m_Chapters; } }
   }
}