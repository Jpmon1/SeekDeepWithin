using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;

namespace SeekDeepWithin.Tests.Mocks
{
   /// <summary>
   /// A mocked database implementation.
   /// </summary>
   public class MockDatabase : ISdwDatabase
   {
      private IRepository <Book> m_Books;
      private IRepository<Author> m_Authors;
      private IRepository<Version> m_Versions;
      private IRepository<SubBook> m_SubBooks;
      private IRepository<Chapter> m_Chapters;
      private IRepository<Passage> m_Passages;
      private IRepository<Link> m_Links;
      private IRepository<Tag> m_Tags;
      private IRepository<Style> m_Styles;
      private IRepository<Source> m_Sources;
      private IRepository<ChapterHeader> m_ChapterHeaders;
      private IRepository<ChapterFooter> m_ChapterFooters;
      private IRepository<PassageHeader> m_PassageHeaders;
      private IRepository<PassageFooter> m_PassageFooters;
      private IRepository<PassageEntry> m_PassageEntries;
      private IRepository<GlossaryTerm> m_GlossaryItems;
      private IRepository<GlossaryEntry> m_GlossaryEntries;

      /// <summary>
      /// Gets the repository for books.
      /// </summary>
      public IRepository <Book> Books
      {
         get { return m_Books ?? (this.m_Books = new MockRepository <Book> ()); }
      }

      /// <summary>
      /// Gets the repository for authors.
      /// </summary>
      public IRepository <Author> Authors
      {
         get { return m_Authors ?? (this.m_Authors = new MockRepository <Author> ()); }
      }

      /// <summary>
      /// Gets the repository for versions.
      /// </summary>
      public IRepository<Version> Versions
      {
         get { return this.m_Versions ?? (this.m_Versions = new MockRepository<Version> ()); }
      }

      /// <summary>
      /// Gets the repository for sub books.
      /// </summary>
      public IRepository<SubBook> SubBooks
      {
         get { return this.m_SubBooks ?? (this.m_SubBooks = new MockRepository<SubBook> ()); }
      }

      /// <summary>
      /// Gets the repository for chapters.
      /// </summary>
      public IRepository<Chapter> Chapters
      {
         get { return this.m_Chapters ?? (this.m_Chapters = new MockRepository<Chapter> ()); }
      }

      /// <summary>
      /// Gets the repository for passages.
      /// </summary>
      public IRepository<Passage> Passages
      {
         get { return this.m_Passages ?? (this.m_Passages = new MockRepository<Passage> ()); }
      }

      /// <summary>
      /// Gets the repository for links.
      /// </summary>
      public IRepository<Link> Links
      {
         get { return this.m_Links ?? (this.m_Links = new MockRepository<Link> ()); }
      }

      /// <summary>
      /// Gets the repository for tags.
      /// </summary>
      public IRepository<Tag> Tags
      {
         get { return this.m_Tags ?? (this.m_Tags = new MockRepository<Tag> ()); }
      }

      /// <summary>
      /// Gets the repository for styles.
      /// </summary>
      public IRepository<Style> Styles
      {
         get { return this.m_Styles ?? (this.m_Styles = new MockRepository<Style> ()); }
      }

      /// <summary>
      /// Gets the repository for sources.
      /// </summary>
      public IRepository<Source> Sources
      {
         get { return this.m_Sources ?? (this.m_Sources = new MockRepository<Source> ()); }
      }

      /// <summary>
      /// Gets the repository for Chapter headers.
      /// </summary>
      public IRepository<ChapterHeader> ChapterHeaders
      {
         get { return this.m_ChapterHeaders ?? (this.m_ChapterHeaders = new MockRepository<ChapterHeader> ()); }
      }

      /// <summary>
      /// Gets the repository for Chapter footers.
      /// </summary>
      public IRepository<ChapterFooter> ChapterFooters
      {
         get { return this.m_ChapterFooters ?? (this.m_ChapterFooters = new MockRepository<ChapterFooter> ()); }
      }

      /// <summary>
      /// Gets the repository for Passage headers.
      /// </summary>
      public IRepository<PassageHeader> PassageHeaders
      {
         get { return this.m_PassageHeaders ?? (this.m_PassageHeaders = new MockRepository<PassageHeader> ()); }
      }

      /// <summary>
      /// Gets the repository for Passage footers.
      /// </summary>
      public IRepository<PassageFooter> PassageFooters
      {
         get { return this.m_PassageFooters ?? (this.m_PassageFooters = new MockRepository<PassageFooter> ()); }
      }

      /// <summary>
      /// Gets the repository for passage entries.
      /// </summary>
      public IRepository<PassageEntry> PassageEntries
      {
         get { return this.m_PassageEntries ?? (this.m_PassageEntries = new MockRepository<PassageEntry> ()); }
      }

      /// <summary>
      /// Gets the repository for passages.
      /// </summary>
      public IRepository<GlossaryTerm> GlossaryTerms
      {
         get { return this.m_GlossaryItems ?? (this.m_GlossaryItems = new MockRepository<GlossaryTerm> ()); }
      }

      /// <summary>
      /// Gets the repository for passages.
      /// </summary>
      public IRepository<GlossaryEntry> GlossaryEntries
      {
         get { return this.m_GlossaryEntries ?? (this.m_GlossaryEntries = new MockRepository<GlossaryEntry> ()); }
      }

      /// <summary>
      /// Saves all changes.
      /// </summary>
      public void Save () {}

      /// <summary>
      /// Sets the values of the given object, with the given values.
      /// </summary>
      /// <param name="item">The database item to set the values for.</param>
      /// <param name="values">The values to update the item with.</param>
      public void SetValues (object item, object values) {}

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose ()
      {
      }
   }
}
