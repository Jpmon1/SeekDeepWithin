using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Tests.Mocks
{
   /// <summary>
   /// A mocked database implementation.
   /// </summary>
   public class MockDatabase : ISdwDatabase
   {
      private IRepository <Book> m_Books;
      private IRepository<Version> m_Versions;
      private IRepository<Chapter> m_Chapters;
      private IRepository<Passage> m_Passages;
      private IRepository<Link> m_Links;
      private IRepository<Style> m_Styles;
      private IRepository<AmazonItem> m_AmazonItems;
      private IRepository<PassageEntry> m_PassageEntries;
      private IRepository<Term> m_GlossaryTerms;
      private IRepository<TermItem> m_GlossaryItems;
      private IRepository<TermItemEntry> m_GlossaryEntries;
      private IRepository <VersionSubBook> m_VersionSubBooks;
      private IRepository<SubBookChapter> m_SubBookChapters;
      private IRepository<TermItemSource> m_GlossaryItemSources;

      /// <summary>
      /// Gets the repository for books.
      /// </summary>
      public IRepository <Book> Books
      {
         get { return m_Books ?? (this.m_Books = new MockRepository <Book> ()); }
      }

      /// <summary>
      /// Gets the repository for glossary item sources.
      /// </summary>
      public IRepository<TermItemSource> TermItemSources
      {
         get { return this.m_GlossaryItemSources ?? (this.m_GlossaryItemSources = new MockRepository<TermItemSource> ()); }
      }

      /// <summary>
      /// Gets the repository for passage entries.
      /// </summary>
      public IRepository<AmazonItem> AmazonItems
      {
         get { return this.m_AmazonItems ?? (this.m_AmazonItems = new MockRepository<AmazonItem> ()); }
      }

      /// <summary>
      /// Gets the repository for versions.
      /// </summary>
      public IRepository<Version> Versions
      {
         get { return this.m_Versions ?? (this.m_Versions = new MockRepository<Version> ()); }
      }

      /// <summary>
      /// Gets the repository for versions subbook table.
      /// </summary>
      public IRepository <VersionSubBook> VersionSubBooks
      {
         get { return m_VersionSubBooks ?? (m_VersionSubBooks = new MockRepository <VersionSubBook> ()); }
      }

      /// <summary>
      /// Gets the repository for subbook chapter table.
      /// </summary>
      public IRepository <SubBookChapter> SubBookChapters
      {
         get { return m_SubBookChapters ?? (this.m_SubBookChapters = new MockRepository <SubBookChapter> ()); }
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
      /// Gets the repository for styles.
      /// </summary>
      public IRepository<Style> Styles
      {
         get { return this.m_Styles ?? (this.m_Styles = new MockRepository<Style> ()); }
      }

      /// <summary>
      /// Gets the repository for passage entries.
      /// </summary>
      public IRepository<PassageEntry> PassageEntries
      {
         get { return this.m_PassageEntries ?? (this.m_PassageEntries = new MockRepository<PassageEntry> ()); }
      }

      /// <summary>
      /// Gets the repository for glossary terms.
      /// </summary>
      public IRepository<Term> Terms
      {
         get { return this.m_GlossaryTerms ?? (this.m_GlossaryTerms = new MockRepository<Term> ()); }
      }

      /// <summary>
      /// Gets the repository for glossary items.
      /// </summary>
      public IRepository<TermItem> TermItems
      {
         get { return this.m_GlossaryItems ?? (this.m_GlossaryItems = new MockRepository<TermItem> ()); }
      }

      /// <summary>
      /// Gets the repository for glossary entries.
      /// </summary>
      public IRepository<TermItemEntry> TermItemEntries
      {
         get { return this.m_GlossaryEntries ?? (this.m_GlossaryEntries = new MockRepository<TermItemEntry> ()); }
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
