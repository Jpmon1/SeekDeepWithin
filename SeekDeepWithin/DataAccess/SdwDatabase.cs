using System;
using SeekDeepWithin.Domain;

namespace SeekDeepWithin.DataAccess
{
   /// <summary>
   /// Access to the seek deep within database.
   /// </summary>
   public class SdwDatabase : ISdwDatabase
   {
      private bool m_Disposed;
      private IRepository<Book> m_Books;
      private IRepository<Author> m_Authors;
      private IRepository<Domain.Version> m_Versions;
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
      private IRepository<GlossaryTerm> m_GlossarrTerms;
      private IRepository<GlossaryEntry> m_GlossaryEntries;
      private readonly SdwDbContext m_Db = new SdwDbContext ();

      /// <summary>
      /// Gets the repository for books.
      /// </summary>
      public IRepository <Book> Books
      {
         get { return this.m_Books ?? (this.m_Books = new Repository <Book> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for authors.
      /// </summary>
      public IRepository <Author> Authors
      {
         get { return this.m_Authors ?? (this.m_Authors = new Repository<Author> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for versions.
      /// </summary>
      public IRepository<Domain.Version> Versions
      {
         get { return this.m_Versions ?? (this.m_Versions = new Repository<Domain.Version> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for subbooks.
      /// </summary>
      public IRepository<SubBook> SubBooks
      {
         get { return this.m_SubBooks ?? (this.m_SubBooks = new Repository<SubBook> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for chapters.
      /// </summary>
      public IRepository<Chapter> Chapters
      {
         get { return this.m_Chapters ?? (this.m_Chapters = new Repository<Chapter> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for passages.
      /// </summary>
      public IRepository<GlossaryTerm> GlossaryTerms
      {
         get { return this.m_GlossarrTerms ?? (this.m_GlossarrTerms = new Repository<GlossaryTerm> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for glossary entries.
      /// </summary>
      public IRepository<GlossaryEntry> GlossaryEntries
      {
         get { return this.m_GlossaryEntries ?? (this.m_GlossaryEntries = new Repository<GlossaryEntry> (m_Db)); }
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
      /// Gets the repository for tags.
      /// </summary>
      public IRepository<Tag> Tags
      {
         get { return this.m_Tags ?? (this.m_Tags = new Repository<Tag> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for styles.
      /// </summary>
      public IRepository<Style> Styles
      {
         get { return this.m_Styles ?? (this.m_Styles = new Repository<Style> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for sources.
      /// </summary>
      public IRepository<Source> Sources
      {
         get { return this.m_Sources ?? (this.m_Sources = new Repository<Source> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for Chapter headers.
      /// </summary>
      public IRepository<ChapterHeader> ChapterHeaders
      {
         get { return this.m_ChapterHeaders ?? (this.m_ChapterHeaders = new Repository<ChapterHeader> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for Chapter footers.
      /// </summary>
      public IRepository<ChapterFooter> ChapterFooters
      {
         get { return this.m_ChapterFooters ?? (this.m_ChapterFooters = new Repository<ChapterFooter> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for Passage headers.
      /// </summary>
      public IRepository<PassageHeader> PassageHeaders
      {
         get { return this.m_PassageHeaders ?? (this.m_PassageHeaders = new Repository<PassageHeader> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for Passage footers.
      /// </summary>
      public IRepository<PassageFooter> PassageFooters
      {
         get { return this.m_PassageFooters ?? (this.m_PassageFooters = new Repository<PassageFooter> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for passage entries.
      /// </summary>
      public IRepository<PassageEntry> PassageEntries
      {
         get { return this.m_PassageEntries ?? (this.m_PassageEntries = new Repository<PassageEntry> (m_Db)); }
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
