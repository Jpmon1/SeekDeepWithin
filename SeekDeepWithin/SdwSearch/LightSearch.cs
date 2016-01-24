using System;
using System.Collections.Generic;
using System.Globalization;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.SdwSearch
{
   /// <summary>
   /// Search functions for light.
   /// </summary>
   public class LightSearch
   {
      private static FSDirectory s_Directory;

      /// <summary>
      /// Gets the lucene directory for glossary entries.
      /// </summary>
      private static FSDirectory Directory { get { return SearchCommon.InitDirectory (ref s_Directory, "light"); } }

      /// <summary>
      /// Queries the indexed data.
      /// </summary>
      /// <param name="token">Toekn to search for.</param>
      public static Dictionary<int, string> AutoComplete (string token)
      {
         return Query (0, 12, token);
      }

      /// <summary>
      /// Queries the indexed data.
      /// </summary>
      /// <param name="start">The starting index.</param>
      /// <param name="count">The number of items to return.</param>
      /// <param name="text">The text to query for.</param>
      public static Dictionary<int, string> Query (int start, int count, string text)
      {
         text = text.Trim ();
         var result = new Dictionary<int, string> ();
         if (string.IsNullOrEmpty (text))
            return result;
         using (var searcher = new IndexSearcher (Directory, true)) {
            var reader = IndexReader.Open (Directory, true);
            var collector = TopScoreDocCollector.Create (count, true);
            var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
            var parser = new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "text", analyzer);
            var qText = text.IndexOf (' ') > 0
               ? string.Format ("(text:\"{0}\")", text)
               : string.Format ("(text:{0}*) OR (text:{0}~0.5)", text);
            var query = SearchCommon.ParseQuery (qText, parser);
            searcher.Search (query, collector);
            var docs = collector.TopDocs (start, count).ScoreDocs;
            foreach (var scoreDoc in docs) {
               var doc = reader.Document (scoreDoc.Doc);
               var id = Convert.ToInt32 (doc.Get ("Id"));
               var t = doc.Get ("text");
               result.Add(id, t);
            }
            reader.Dispose ();
            analyzer.Close ();
         }
         return result;
      }

      /// <summary>
      /// Adds or Updates the index for the given item.
      /// </summary>
      /// <param name="item">Item to update index for.</param>
      public static void AddOrUpdateIndex (Light item)
      {
         AddOrUpdateIndex (new [] { item });
      }

      /// <summary>
      /// Adds or Updates the index for the given items.
      /// </summary>
      /// <param name="items">Items to update index for.</param>
      public static void AddOrUpdateIndex (IEnumerable<Light> items)
      {
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         using (var writer = new IndexWriter (Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED)) {
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
         using (var writer = new IndexWriter (Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED)) {
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
         using (var writer = new IndexWriter (Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED)) {
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
         try {
            var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter (Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
               writer.DeleteAll ();
            analyzer.Close ();
         } catch (Exception) {
            return false;
         }
         return true;
      }

      /// <summary>
      /// Adds an index for the given item.
      /// </summary>
      /// <param name="light">Item to index.</param>
      /// <param name="writer">Index writer.</param>
      private static void AddIndex (Light light, IndexWriter writer)
      {
         // remove older index entry
         var id = light.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Lucene.Net.Index.Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("text", light.Text ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED,
            Field.TermVector.WITH_POSITIONS_OFFSETS));
         writer.AddDocument (doc);
      }
   }
}