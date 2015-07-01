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
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;
using Term = Lucene.Net.Index.Term;

namespace SeekDeepWithin.SdwSearch
{
   /// <summary>
   /// Search functions for glossary entries.
   /// </summary>
   public class GlossarySearch
   {
      private static FSDirectory s_Directory;

      /// <summary>
      /// Gets the lucene directory for glossary entries.
      /// </summary>
      private static FSDirectory Directory { get { return SearchCommon.InitDirectory (ref s_Directory, "glossary"); } }

      /// <summary>
      /// Queries the indexed data.
      /// </summary>
      /// <param name="search">Search query options.</param>
      /// <param name="results">Search results.</param>
      /// <param name="host">The host web address.</param>
      public static void Query (SearchQueryViewModel search, SearchResultsViewModel results, string host)
      {
         using (var searcher = new IndexSearcher (Directory, true))
         {
            var reader = IndexReader.Open (Directory, true);
            var q = "(text:{0})";
            if (search.DoHeaders)
               q += " OR (header:{0})";
            if (search.DoFooters)
               q += " OR (footer:{0})";
            var collector = TopScoreDocCollector.Create (Math.Max (SearchCommon.HITS_LIMIT, search.PageSize * search.Page), true);
            var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
            var parser = new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "text", analyzer);
            var query = SearchCommon.ParseQuery (SearchCommon.BuildQuery (search, q), parser);
            searcher.Search (query, collector);
            var start = (search.Page - 1) * search.PageSize;
            var docs = collector.TopDocs (start, search.PageSize).ScoreDocs;
            results.TotalHits = collector.TotalHits;
            results.SearchType = SearchType.Glossary;
            results.Title = "Glossary Entries";
            results.Start = start + 1;
            results.End = Math.Min (results.TotalHits, search.PageSize * search.Page);
            foreach (var scoreDoc in docs)
            {
               var doc = reader.Document (scoreDoc.Doc);
               var id = doc.Get ("Id");
               var title = doc.Get ("term");
               var termId = doc.Get ("termId");
               var result = new SearchResult
               {
                  Id = id,
                  Title = title.Highlight (search),
                  Url = string.Format ("{0}/Term/{1}", host, termId),
                  Description = doc.Get ("text").Highlight (search)
               };
               results.Add (result);
            }
            reader.Dispose ();
            analyzer.Close ();
         }
      }

      /// <summary>
      /// Adds or Updates the index for the given item.
      /// </summary>
      /// <param name="item">Item to update index for.</param>
      public static void AddOrUpdateIndex (TermItemEntry item)
      {
         AddOrUpdateIndex (new[] { item });
      }

      /// <summary>
      /// Adds or Updates the index for the given items.
      /// </summary>
      /// <param name="items">Items to update index for.</param>
      public static void AddOrUpdateIndex (IEnumerable<TermItemEntry> items)
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
      /// <param name="entry">Item to index.</param>
      /// <param name="writer">Index writer.</param>
      private static void AddIndex (TermItemEntry entry, IndexWriter writer)
      {
         // remove older index entry
         var id = entry.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("termId", entry.Item.Term.Id.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("text", entry.Text, Field.Store.YES, Field.Index.ANALYZED));
         doc.Add (new Field ("term", entry.Item.Term.Name, Field.Store.YES, Field.Index.ANALYZED));
         if (entry.Item.Source != null)
            doc.Add (new Field ("source", entry.Item.Source.Name, Field.Store.YES, Field.Index.ANALYZED));
         if (entry.Header != null)
            doc.Add (new Field ("header", entry.Header.Text, Field.Store.YES, Field.Index.ANALYZED));
         if (entry.Footers != null && entry.Footers.Count > 0)
         {
            doc.Add (new Field ("footer", entry.Footers.Select (f => f.Text).Aggregate ((i, j) => i + " " + j),
               Field.Store.YES, Field.Index.ANALYZED));
         }
         // add entry to index
         writer.AddDocument (doc);
      }
   }
}