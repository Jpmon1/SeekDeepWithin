using System;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.DataAccess
{
   public interface ISdwDatabase : IDisposable
   {
      /// <summary>
      /// Gets the repository for books.
      /// </summary>
      IRepository <Book> Books { get; }

      /// <summary>
      /// Gets the repository for authors.
      /// </summary>
      IRepository<Author> Authors { get; }

      /// <summary>
      /// Gets the repository for versions.
      /// </summary>
      IRepository<Pocos.Version> Versions { get; }

      /// <summary>
      /// Gets the repository for subbooks.
      /// </summary>
      IRepository<SubBook> SubBooks { get; }

      /// <summary>
      /// Gets the repository for chapters.
      /// </summary>
      IRepository<Chapter> Chapters { get; }

      /// <summary>
      /// Gets the repository for links.
      /// </summary>
      IRepository<Link> Links { get; }

      /// <summary>
      /// Gets the repository for tags.
      /// </summary>
      IRepository<Tag> Tags { get; }

      /// <summary>
      /// Gets the repository for styles.
      /// </summary>
      IRepository<Style> Styles { get; }

      /// <summary>
      /// Gets the repository for sources.
      /// </summary>
      IRepository<Source> Sources { get; }

      /// <summary>
      /// Gets the repository for passage entries.
      /// </summary>
      IRepository<PassageEntry> PassageEntries { get; }

      /// <summary>
      /// Gets the repository for passages.
      /// </summary>
      IRepository<Passage> Passages { get; }

      /// <summary>
      /// Gets the repository for Chapter headers.
      /// </summary>
      IRepository<ChapterHeader> ChapterHeaders { get; }

      /// <summary>
      /// Gets the repository for Chapter footers.
      /// </summary>
      IRepository<ChapterFooter> ChapterFooters { get; }

      /// <summary>
      /// Gets the repository for Passage headers.
      /// </summary>
      IRepository<PassageHeader> PassageHeaders { get; }

      /// <summary>
      /// Gets the repository for Passage footers.
      /// </summary>
      IRepository<PassageFooter> PassageFooters { get; }

      /// <summary>
      /// Gets the repository for glossary terms.
      /// </summary>
      IRepository <GlossaryTerm> GlossaryTerms { get; }

      /// <summary>
      /// Gets the repository for glossary items.
      /// </summary>
      IRepository<GlossaryItem> GlossaryItems { get; }

      /// <summary>
      /// Gets the repository for glossary entries.
      /// </summary>
      IRepository<GlossaryEntry> GlossaryEntries { get; }

      /// <summary>
      /// Saves all changes.
      /// </summary>
      void Save ();

      /// <summary>
      /// Sets the values of the given object, with the given values.
      /// </summary>
      /// <param name="item">The database item to set the values for.</param>
      /// <param name="values">The values to update the item with.</param>
      void SetValues (object item, object values);
   }
}
