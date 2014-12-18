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
      private Repository<Book> m_BookRepository;
      private Repository<Author> m_AuthorRepository;
      private Repository<Domain.Version> m_VersionRepository;
      private Repository<SubBook> m_SubBookRepository;
      private Repository<Chapter> m_ChapterRepository;
      private Repository<Passage> m_PassageRepository;
      private Repository<Link> m_LinkRepository;
      private Repository<Tag> m_TagRepository;
      private Repository<Source> m_SourceRepository;
      private Repository<PassageEntry> m_PassageEntryRepository;
      private Repository<GlossaryTerm> m_GlossaryItemRepository;
      private Repository<GlossaryEntry> m_GlossaryEntryRepository;
      private readonly SdwDbContext m_Db = new SdwDbContext ();

      /// <summary>
      /// Gets the repository for books.
      /// </summary>
      public IRepository <Book> Books
      {
         get { return this.m_BookRepository ?? (this.m_BookRepository = new Repository <Book> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for authors.
      /// </summary>
      public IRepository <Author> Authors
      {
         get { return this.m_AuthorRepository ?? (this.m_AuthorRepository = new Repository<Author> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for versions.
      /// </summary>
      public IRepository<Domain.Version> Versions
      {
         get { return this.m_VersionRepository ?? (this.m_VersionRepository = new Repository<Domain.Version> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for subbooks.
      /// </summary>
      public IRepository<SubBook> SubBooks
      {
         get { return this.m_SubBookRepository ?? (this.m_SubBookRepository = new Repository<SubBook> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for chapters.
      /// </summary>
      public IRepository<Chapter> Chapters
      {
         get { return this.m_ChapterRepository ?? (this.m_ChapterRepository = new Repository<Chapter> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for passages.
      /// </summary>
      public IRepository<GlossaryTerm> GlossaryTerms
      {
         get { return this.m_GlossaryItemRepository ?? (this.m_GlossaryItemRepository = new Repository<GlossaryTerm> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for glossary entries.
      /// </summary>
      public IRepository<GlossaryEntry> GlossaryEntries
      {
         get { return this.m_GlossaryEntryRepository ?? (this.m_GlossaryEntryRepository = new Repository<GlossaryEntry> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for passages.
      /// </summary>
      public IRepository<Passage> Passages
      {
         get { return this.m_PassageRepository ?? (this.m_PassageRepository = new Repository<Passage> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for links.
      /// </summary>
      public IRepository<Link> Links
      {
         get { return this.m_LinkRepository ?? (this.m_LinkRepository = new Repository<Link> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for tags.
      /// </summary>
      public IRepository<Tag> Tags
      {
         get { return this.m_TagRepository ?? (this.m_TagRepository = new Repository<Tag> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for sources.
      /// </summary>
      public IRepository<Source> Sources
      {
         get { return this.m_SourceRepository ?? (this.m_SourceRepository = new Repository<Source> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for passage entries.
      /// </summary>
      public IRepository<PassageEntry> PassageEntries
      {
         get { return this.m_PassageEntryRepository ?? (this.m_PassageEntryRepository = new Repository<PassageEntry> (m_Db)); }
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
