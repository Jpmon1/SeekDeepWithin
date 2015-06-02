﻿using System;
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

namespace SeekDeepWithin.SdwSearch
{
   /// <summary>
   /// Search functions for books.
   /// </summary>
   public static class BookSearch
   {
      private static FSDirectory s_Directory;

      /// <summary>
      /// Gets the lucene directory for books.
      /// </summary>
      private static FSDirectory Directory { get { return SearchCommon.InitDirectory (ref s_Directory, "book"); } }

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
            var collector = TopScoreDocCollector.Create (Math.Max (SearchCommon.HITS_LIMIT, search.PageSize * search.Page), true);
            var analyzer = new StandardAnalyzer (global::Lucene.Net.Util.Version.LUCENE_30);
            var parser = new QueryParser (global::Lucene.Net.Util.Version.LUCENE_30, "title", analyzer);
            var query = SearchCommon.ParseQuery (SearchCommon.BuildQuery (search,
               "(title:{1}) OR (subtitle:{0}) OR (summary:{0}) OR (tags:{0}) OR (versions:{0})"), parser);
            searcher.Search (query, collector);
            var start = (search.Page - 1) * search.PageSize;
            var docs = collector.TopDocs (start, search.PageSize).ScoreDocs;
            results.TotalHits = collector.TotalHits;
            results.SearchType = SearchType.Book;
            results.Title = "Books";
            results.Start = start + 1;
            results.End = Math.Min(results.TotalHits, search.PageSize * search.Page);
            foreach (var scoreDoc in docs)
            {
               var doc = reader.Document (scoreDoc.Doc);
               var id = doc.Get ("Id");
               var title = doc.Get ("title");
               var result = new SearchResult
               {
                  Id = id,
                  Title = string.Format ("<a href=\"{0}/Book/Details/{1}\">{2}</a>", host, id, title.Highlight (search)),
                  Description = doc.Get ("summary").Highlight(search)
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
      public static void AddOrUpdateIndex (Book item)
      {
         AddOrUpdateIndex (new[] { item });
      }

      /// <summary>
      /// Adds or Updates the index for the given items.
      /// </summary>
      /// <param name="items">Items to update index for.</param>
      public static void AddOrUpdateIndex (IEnumerable<Book> items)
      {
         var analyzer = new StandardAnalyzer (global::Lucene.Net.Util.Version.LUCENE_30);
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
         var analyzer = new StandardAnalyzer (global::Lucene.Net.Util.Version.LUCENE_30);
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
         var analyzer = new StandardAnalyzer (global::Lucene.Net.Util.Version.LUCENE_30);
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
            var analyzer = new StandardAnalyzer (global::Lucene.Net.Util.Version.LUCENE_30);
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
      /// <param name="book">Item to index.</param>
      /// <param name="writer">Index writer.</param>
      private static void AddIndex (Book book, IndexWriter writer)
      {
         // remove older index entry
         var id = book.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("title", book.Title ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         doc.Add (new Field ("subtitle", book.SubTitle ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         doc.Add (new Field ("summary", book.Summary ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         if (book.Tags.Count > 0)
         {
            doc.Add (new Field ("tags", book.Tags.Select (t => t.Tag.Name)
                  .Aggregate ((i, j) => i + " " + j), Field.Store.YES, Field.Index.ANALYZED));
         }
         if (book.Versions.Count > 0)
         {
            doc.Add (new Field ("versions", book.Versions.Select (t => t.Title + " " + t.PublishDate)
                  .Aggregate ((i, j) => i + " | " + j), Field.Store.YES, Field.Index.ANALYZED));
         }
         // add entry to index
         writer.AddDocument (doc);
      }
   }
}