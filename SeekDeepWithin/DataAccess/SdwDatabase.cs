using System;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.DataAccess
{
   /// <summary>
   /// Access to the seek deep within database.
   /// </summary>
   public class SdwDatabase : ISdwDatabase
   {
      private bool m_Disposed;
      private IRepository<Light> m_Lights;
      private IRepository<Love> m_Loves;
      private IRepository<Truth> m_Truths;
      private IRepository<FormatRegex> m_RegexFormats;
      private IRepository<Book> m_Books;
      private IRepository<Pocos.Version> m_Versions;
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
      private readonly SdwDbContext m_Db = new SdwDbContext ();

      /// <summary>
      /// Gets the repository for light.
      /// </summary>
      public IRepository<Light> Light
      {
         get { return this.m_Lights ?? (this.m_Lights = new Repository<Light> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for love.
      /// </summary>
      public IRepository<Love> Love
      {
         get { return this.m_Loves ?? (this.m_Loves = new Repository<Love> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for truth.
      /// </summary>
      public IRepository<Truth> Truth
      {
         get { return this.m_Truths ?? (this.m_Truths = new Repository<Truth> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for formatting regular expressions.
      /// </summary>
      public IRepository<FormatRegex> RegexFormats
      {
         get { return this.m_RegexFormats ?? (this.m_RegexFormats = new Repository<FormatRegex> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for books.
      /// </summary>
      public IRepository<Book> Books
      {
         get { return this.m_Books ?? (this.m_Books = new Repository<Book> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for glossary item sources.
      /// </summary>
      public IRepository<TermItemSource> TermItemSources
      {
         get { return this.m_GlossaryItemSources ?? (this.m_GlossaryItemSources = new Repository<TermItemSource> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for versions.
      /// </summary>
      public IRepository<Pocos.Version> Versions
      {
         get { return this.m_Versions ?? (this.m_Versions = new Repository<Pocos.Version> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for versions subbook table.
      /// </summary>
      public IRepository <VersionSubBook> VersionSubBooks
      {
         get { return m_VersionSubBooks ?? (this.m_VersionSubBooks = new Repository <VersionSubBook> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for subbook chapter table.
      /// </summary>
      public IRepository <SubBookChapter> SubBookChapters
      {
         get { return m_SubBookChapters ?? (this.m_SubBookChapters = new Repository <SubBookChapter> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for chapters.
      /// </summary>
      public IRepository<Chapter> Chapters
      {
         get { return this.m_Chapters ?? (this.m_Chapters = new Repository<Chapter> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for glossary terms.
      /// </summary>
      public IRepository<Term> Terms
      {
         get { return this.m_GlossaryTerms ?? (this.m_GlossaryTerms = new Repository<Term> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for glossary items.
      /// </summary>
      public IRepository<TermItem> TermItems
      {
         get { return this.m_GlossaryItems ?? (this.m_GlossaryItems = new Repository<TermItem> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for glossary entries.
      /// </summary>
      public IRepository<TermItemEntry> TermItemEntries
      {
         get { return this.m_GlossaryEntries ?? (this.m_GlossaryEntries = new Repository<TermItemEntry> (this.m_Db)); }
      }

      /// <summary>
      /// Gets the repository for passages.
      /// </summary>
      public IRepository<Passage> Passages
      {
         get { return this.m_Passages ?? (this.m_Passages = new Repository<Passage> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for links.
      /// </summary>
      public IRepository<Link> Links
      {
         get { return this.m_Links ?? (this.m_Links = new Repository<Link> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for styles.
      /// </summary>
      public IRepository<Style> Styles
      {
         get { return this.m_Styles ?? (this.m_Styles = new Repository<Style> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for passage entries.
      /// </summary>
      public IRepository<PassageEntry> PassageEntries
      {
         get { return this.m_PassageEntries ?? (this.m_PassageEntries = new Repository<PassageEntry> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for amazon items.
      /// </summary>
      public IRepository<AmazonItem> AmazonItems
      {
         get { return this.m_AmazonItems ?? (this.m_AmazonItems = new Repository<AmazonItem> (m_Db)); }
      }

      /// <summary>
      /// Sets the values of the given object, with the given values.
      /// </summary>
      /// <param name="item">The database item to set the values for.</param>
      /// <param name="values">The values to update the item with.</param>
      public void SetValues (object item, object values)
      {
         this.m_Db.Entry (item).CurrentValues.SetValues (values);
      }

      /// <summary>
      /// Saves all changes.
      /// </summary>
      public void Save ()
      {
         this.m_Db.SaveChanges ();
      }

      /// <summary>
      /// Disposes of any objects.
      /// </summary>
      public void Dispose ()
      {
         this.Dispose (true);
         GC.SuppressFinalize (this);
      }

      /// <summary>
      /// Disploses of the object, if not already disposed.
      /// </summary>
      /// <param name="disposing">True to dispose.</param>
      protected virtual void Dispose (bool disposing)
      {
         if (!this.m_Disposed)
         {
            if (disposing)
            {
               this.m_Db.Dispose ();
            }
         }
         this.m_Disposed = true;
      }
   }
}
