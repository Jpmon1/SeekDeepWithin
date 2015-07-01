using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.SdwSearch
{
   public class AbbrevSearch
   {
      private static FSDirectory s_Directory;

      /// <summary>
      /// Gets the lucene directory for glossary entries.
      /// </summary>
      private static FSDirectory Directory { get { return SearchCommon.InitDirectory (ref s_Directory, "abbrev"); } }

      /// <summary>
      /// Queries the indexed data.
      /// </summary>
      /// <param name="abbreviation">The abbreviation to search for.</param>
      public static int Query (string abbreviation)
      {
         int result = -1;
         if (string.IsNullOrWhiteSpace (abbreviation))
            return result;
         using (var searcher = new IndexSearcher (Directory, true))
         {
            var reader = IndexReader.Open (Directory, true);
            var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
            var parser = new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "Id", analyzer);
            var query = SearchCommon.ParseQuery (abbreviation, parser);
            var scoreDoc = searcher.Search (query, null, SearchCommon.HITS_LIMIT).ScoreDocs.FirstOrDefault();
            if (scoreDoc != null)
            {
               var doc = reader.Document (scoreDoc.Doc);
               var id = doc.Get ("subBookId");
               result = Convert.ToInt32 (id);
            }
            reader.Dispose ();
            analyzer.Close ();
         }
         return result;
      }

      /// <summary>
      /// Queries the indexed data.
      /// </summary>
      /// <param name="termId">The term id to get abbreviations for.</param>
      public static List<string> Get (int termId)
      {
         var result = new List<string> ();
         using (var searcher = new IndexSearcher (Directory, true))
         {
            var reader = IndexReader.Open (Directory, true);
            var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
            var parser = new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "termId", analyzer);
            var query = SearchCommon.ParseQuery (termId.ToString(CultureInfo.InvariantCulture), parser);
            var docs = searcher.Search (query, null, SearchCommon.HITS_LIMIT).ScoreDocs;
            result.AddRange (docs.Select (scoreDoc => reader.Document (scoreDoc.Doc)).Select (doc => doc.Get ("Id")));
            reader.Dispose ();
            analyzer.Close ();
         }
         return result;
      }

      /// <summary>
      /// Adds or Updates the index for the given item.
      /// </summary>
      /// <param name="item">Item to update index for.</param>
      /// <param name="abbreviation">The abbreviation to use.</param>
      public static void AddOrUpdateIndex (VersionSubBook item, string abbreviation)
      {
         AddOrUpdateIndex (new[] { item }, abbreviation);
      }

      /// <summary>
      /// Adds or Updates the index for the given items.
      /// </summary>
      /// <param name="items">Items to update index for.</param>
      /// <param name="abbreviation">The abbreviation to use.</param>
      public static void AddOrUpdateIndex (IEnumerable<VersionSubBook> items, string abbreviation)
      {
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         using (var writer = new IndexWriter (Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
         {
            writer.WriteLockTimeout = -1;
            foreach (var item in items) AddIndex (item, writer, abbreviation);
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
      public static void Delete (string recordId)
      {
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         using (var writer = new IndexWriter (Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
         {
            var searchQuery = new TermQuery (new Lucene.Net.Index.Term ("Id", recordId));
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
      /// <param name="subBook">Item to index.</param>
      /// <param name="writer">Index writer.</param>
      /// <param name="abbreviation">The abbreviation to use.</param>
      private static void AddIndex (VersionSubBook subBook, IndexWriter writer, string abbreviation)
      {
         var defaultVersion = subBook.Version.Book.DefaultVersion;
         var dbSubBook = defaultVersion.SubBooks.FirstOrDefault (sb => sb.Term.Id == subBook.Term.Id);
         if (dbSubBook == null) return;

         // remove older index entry
         var searchQuery = new TermQuery (new Lucene.Net.Index.Term ("Id", abbreviation));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         var subBookId = dbSubBook.Id.ToString (CultureInfo.InvariantCulture);
         var termId = dbSubBook.Term.Id.ToString (CultureInfo.InvariantCulture);
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", abbreviation, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("termId", termId, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("subBookId", subBookId, Field.Store.YES, Field.Index.NOT_ANALYZED));
         // add entry to index
         writer.AddDocument (doc);
      }
   }
}