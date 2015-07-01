using System;
using System.Collections.Generic;
using System.Globalization;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SeekDeepWithin.Pocos;
using Term = Lucene.Net.Index.Term;

namespace SeekDeepWithin.SdwSearch
{
   /// <summary>
   /// Search functions for chapters.
   /// </summary>
   public static class ChapterSearch
   {
      private static FSDirectory s_Directory;

      /// <summary>
      /// Gets the lucene directory for chapters.
      /// </summary>
      private static FSDirectory Directory { get { return SearchCommon.InitDirectory (ref s_Directory, "chapter"); } }

      /// <summary>
      /// Adds or Updates the index for the given item.
      /// </summary>
      /// <param name="item">Item to update index for.</param>
      public static void AddOrUpdateIndex (Chapter item)
      {
         AddOrUpdateIndex (new[] { item });
      }

      /// <summary>
      /// Adds or Updates the index for the given items.
      /// </summary>
      /// <param name="items">Items to update index for.</param>
      public static void AddOrUpdateIndex (IEnumerable<Chapter> items)
      {
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         using (var writer = new IndexWriter (Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
         {
            writer.WriteLockTimeout = -1;
            foreach (var item in items) AddIndex (item, writer);
         }
         analyzer.Close ();
      }

      /// <summary>
      /// Optimizes the lucene database.
      /// </summary>
      public static void Optimize ()
      {
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         using (var writer = new IndexWriter (Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
         {
            analyzer.Close ();
            writer.Optimize ();
         }
      }

      /// <summary>
      /// Clears all data with the given record id.
      /// </summary>
      /// <param name="recordId">Id of item index to delete.</param>
      public static void Delete (int recordId)
      {
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         using (var writer = new IndexWriter (Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
         {
            var searchQuery = new TermQuery (new Term ("Id", recordId.ToString (CultureInfo.InvariantCulture)));
            writer.DeleteDocuments (searchQuery);
            analyzer.Close ();
         }
      }

      /// <summary>
      /// Clears all lucene data.
      /// </summary>
      /// <returns>True if successful, otherwise false.</returns>
      public static bool Clear ()
      {
         try
         {
            var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter (Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
               writer.DeleteAll ();
            analyzer.Close ();
         }
         catch (Exception)
         {
            return false;
         }
         return true;
      }

      /// <summary>
      /// Adds an index for the given item.
      /// </summary>
      /// <param name="chapter">Item to index.</param>
      /// <param name="writer">Index writer.</param>
      private static void AddIndex (Chapter chapter, IndexWriter writer)
      {
         // remove older index entry
         var id = chapter.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("Name", chapter.Name ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         // add entry to index
         writer.AddDocument (doc);
      }
   }
}