using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;
using Directory = Lucene.Net.Store.Directory;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Implements lucene indexing and searching.
   /// http://www.lucenetutorial.com/lucene-query-syntax.html
   /// </summary>
   public static class Search
   {
      private static string s_LuceneDir;
      private static FSDirectory s_TagDirectory;
      private static FSDirectory s_PassageDirectory;
      private static FSDirectory s_WriterDirectory;
      private static FSDirectory s_GlossaryDirectory;
      private static FSDirectory s_TermDirectory;
      private static FSDirectory s_BookDirectory;
      private static FSDirectory s_VersionDirectory;
      private static FSDirectory s_SubBookDirectory;
      private static FSDirectory s_ChapterDirectory;

      private const int HITS_LIMIT = 1000;

      /// <summary>
      /// Performs a search on the given directory.
      /// </summary>
      /// <param name="searchQuery">Query to search for.</param>
      /// <returns>List of matching Ids.</returns>
      public static Dictionary<SearchType, List<int>> Query (SearchQueryViewModel searchQuery)
      {
         if (string.IsNullOrEmpty (searchQuery.Q.Replace ("*", "").Replace ("?", "")))
            return new Dictionary<SearchType, List<int>>();

         var results = new Dictionary <SearchType, List <int>> ();
         var searchTypes = GetSearchTypes (searchQuery);
         var directories = GetDirectories (searchTypes);
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         foreach (var directory in directories)
         {
            var fields = new[] {"Name"};
            if (directory == PassageDirectory)
               fields = GetFields (searchQuery.DoPassHeaders, searchQuery.DoPassFooters);
            else if (directory == GlossaryDirectory)
               fields = GetFields (searchQuery.DoGlossHeaders, searchQuery.DoGlossFooters);
            using (var searcher = new IndexSearcher (directory, true))
            {
               var parser = fields.Length > 1
                  ? new MultiFieldQueryParser (Lucene.Net.Util.Version.LUCENE_30, fields, analyzer)
                  : new QueryParser (Lucene.Net.Util.Version.LUCENE_30, fields [0], analyzer);
               var query = ParseQuery (searchQuery.Q, parser, fields.Length == 1);
               var hits = searcher.Search (query, null, HITS_LIMIT, Sort.RELEVANCE).ScoreDocs;
               var resList = hits.Select (hit => Convert.ToInt32 (searcher.Doc (hit.Doc).Get ("Id"))).ToList ();
               if (resList.Count > 0)
                  results.Add (GetSearchType (directory), resList);
            }
         }
         analyzer.Close ();
         return results;
      }

      /// <summary>
      /// Gets the search type for the given directory.
      /// </summary>
      /// <param name="directory">Directory to get the search type for.</param>
      /// <returns>The search type, default is all.</returns>
      public static SearchType GetSearchType (Directory directory)
      {
         if (directory == BookDirectory)
            return SearchType.Book;
         if (directory == VersionDirectory)
            return SearchType.Version;
         if (directory == SubBookDirectory)
            return SearchType.SubBook;
         if (directory == ChapterDirectory)
            return SearchType.Chapter;
         if (directory == PassageDirectory)
            return SearchType.Passage;
         if (directory == TermDirectory)
            return SearchType.Term;
         if (directory == GlossaryDirectory)
            return SearchType.Glossary;
         if (directory == WriterDirectory)
            return SearchType.Writer;
         if (directory == TagDirectory)
            return SearchType.Tag;

         return SearchType.All;
      }

      /// <summary>
      /// Gets the required search types for the given search query.
      /// </summary>
      /// <param name="query">Query to get types for.</param>
      /// <returns>The list of types to search through.</returns>
      private static IEnumerable <SearchType> GetSearchTypes (SearchQueryViewModel query)
      {
         var types = new List <SearchType> ();
         if (query.DoPassages)
         {
            types.Add (SearchType.Book);
            types.Add (SearchType.Version);
            types.Add (SearchType.SubBook);
            types.Add (SearchType.Chapter);
            types.Add (SearchType.Passage);
         }
         if (query.DoGlossary)
         {
            types.Add (SearchType.Term);
            types.Add (SearchType.Glossary);
         }
         if (query.DoWriters)
            types.Add (SearchType.Writer);
         if (query.DoTags)
            types.Add (SearchType.Tag);
         return types;
      }

      /// <summary>
      /// Gets the fields based on the required data.
      /// </summary>
      /// <param name="headers">True to include headers.</param>
      /// <param name="footers">True to include footers.</param>
      /// <returns>The search fields.</returns>
      private static string[] GetFields (bool headers, bool footers)
      {
         string[] fields = { "Text" };
         if (headers)
         {
            Array.Resize (ref fields, 2);
            fields[1] = "Header";
         }
         if (footers)
         {
            Array.Resize (ref fields, fields.Length + 1);
            fields[fields.Length - 1] = "Footer";
         }
         return fields;
      }

      /// <summary>
      /// Adds or Updates the index for the given item.
      /// </summary>
      /// <param name="item">Item to update index for.</param>
      /// <param name="type">Type of item.</param>
      public static void AddOrUpdateIndex (Object item, SearchType type)
      {
         AddOrUpdateIndex (new[] {item}, type);
      }

      /// <summary>
      /// Adds or Updates the index for the given items.
      /// </summary>
      /// <param name="items">Items to update index for.</param>
      /// <param name="type">Type of items.</param>
      public static void AddOrUpdateIndex (IEnumerable<Object> items, SearchType type)
      {
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         var directory = GetDirectories (type).FirstOrDefault ();
         if (directory == null) return;
         using (var writer = new IndexWriter (directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
         {
            if (type == SearchType.Book)
               foreach (var item in items.Cast<Book> ()) AddIndex (item, writer);
            else if (type == SearchType.Version)
               foreach (var item in items.Cast<SeekDeepWithin.Pocos.Version> ()) AddIndex (item, writer);
            else if (type == SearchType.SubBook)
               foreach (var item in items.Cast<VersionSubBook> ()) AddIndex (item, writer);
            else if (type == SearchType.Chapter)
               foreach (var item in items.Cast<Chapter> ()) AddIndex (item, writer);
            else if (type == SearchType.Passage)
               foreach (var item in items.Cast<PassageEntry> ()) AddIndex (item, writer);
            else if (type == SearchType.Term)
               foreach (var item in items.Cast<GlossaryTerm> ()) AddIndex (item, writer);
            else if (type == SearchType.Glossary)
               foreach (var item in items.Cast<GlossaryEntry> ()) AddIndex (item, writer);
            else if (type == SearchType.Writer)
               foreach (var item in items.Cast<Writer> ()) AddIndex (item, writer);
            else if (type == SearchType.Tag)
               foreach (var item in items.Cast<Tag> ()) AddIndex (item, writer);
            analyzer.Close ();
         }
      }

      /// <summary>
      /// Clears all data with the given record id.
      /// </summary>
      /// <param name="recordId">Id of item index to delete.</param>
      /// <param name="type">Type of item index to delete.</param>
      public static void Clear (int recordId, SearchType type)
      {
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         var directories = GetDirectories (type);
         foreach (var fsDirectory in directories)
         {
            using (var writer = new IndexWriter (fsDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
               var searchQuery = new TermQuery (new Term ("Id", recordId.ToString(CultureInfo.InvariantCulture)));
               writer.DeleteDocuments (searchQuery);
               analyzer.Close ();
            }
         }
      }

      /// <summary>
      /// Gets the directory for the given type.
      /// </summary>
      /// <param name="type">The type to get the directory for.</param>
      /// <returns>The corresponding directory.</returns>
      private static IEnumerable <FSDirectory> GetDirectories (SearchType type)
      {
         return GetDirectories (new[] {type});
      }

      /// <summary>
      /// Gets the directory for the given type.
      /// </summary>
      /// <param name="types">The types to get directories for.</param>
      /// <returns>The corresponding directory.</returns>
      private static IEnumerable <FSDirectory> GetDirectories (IEnumerable<SearchType> types)
      {
         var directories = new List <FSDirectory> ();
         foreach (var type in types)
         {
            if (type == SearchType.Book || type == SearchType.All)
               directories.Add (BookDirectory);
            if (type == SearchType.Version || type == SearchType.All)
               directories.Add (VersionDirectory);
            if (type == SearchType.SubBook || type == SearchType.All)
               directories.Add (SubBookDirectory);
            if (type == SearchType.Chapter || type == SearchType.All)
               directories.Add (ChapterDirectory);
            if (type == SearchType.Passage || type == SearchType.All)
               directories.Add (PassageDirectory);
            if (type == SearchType.Term || type == SearchType.All)
               directories.Add (TermDirectory);
            if (type == SearchType.Glossary || type == SearchType.All)
               directories.Add (GlossaryDirectory);
            if (type == SearchType.Writer || type == SearchType.All)
               directories.Add (WriterDirectory);
            if (type == SearchType.Tag || type == SearchType.All)
               directories.Add (TagDirectory);
         }
         return directories.Distinct();
      }

      /// <summary>
      /// Gets the search type for the given string.
      /// </summary>
      /// <param name="type">String representation of search type.</param>
      /// <returns>The search type, default is all.</returns>
      public static SearchType GetSearchType (string type)
      {
         type = type ?? string.Empty;
         switch (type.ToLower())
         {
            case "book":
               return SearchType.Book;
            case "version":
               return SearchType.Version;
            case "subbook":
               return SearchType.SubBook;
            case "chapter":
               return SearchType.Chapter;
            case "passage":
               return SearchType.Passage;
            case "term":
               return SearchType.Term;
            case "glossary":
               return SearchType.Glossary;
            case "writer":
               return SearchType.Writer;
            case "tag":
               return SearchType.Tag;
            default:
               return SearchType.All;
         }
      }

      /// <summary>
      /// Clears all lucene data.
      /// </summary>
      /// <returns>True if successful, otherwise false.</returns>
      public static bool Clear (SearchType type = SearchType.All)
      {
         try
         {
            var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
            var directories = GetDirectories (type);
            foreach (var directory in directories)
            {
               using (var writer = new IndexWriter (directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                  writer.DeleteAll ();
            }
            analyzer.Close();
         }
         catch (Exception)
         {
            return false;
         }
         return true;
      }

      /// <summary>
      /// Optimizes the lucene database.
      /// </summary>
      /// <param name="type"></param>
      public static void Optimize (SearchType type = SearchType.All)
      {
         var directories = GetDirectories (type);
         var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
         foreach (var directory in directories)
         {
            using (var writer = new IndexWriter (directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
               writer.Optimize ();
         }
         analyzer.Close ();
      }

      private static void AddIndex (PassageEntry entry, IndexWriter writer)
      {
         // remove older index entry
         var id = entry.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("Text", entry.Passage.Text, Field.Store.YES, Field.Index.ANALYZED));
         if (entry.Headers.Count > 0)
            doc.Add (new Field ("Header", entry.Headers.Select (h => h.Text).Aggregate ((i, j) => i + " " + j), Field.Store.YES, Field.Index.ANALYZED));
         if (entry.Footers.Count > 0)
            doc.Add (new Field ("Footer", entry.Footers.Select (f => f.Text).Aggregate ((i, j) => i + " " + j), Field.Store.YES, Field.Index.ANALYZED));
         // add entry to index
         writer.AddDocument (doc);
      }

      private static void AddIndex (GlossaryEntry entry, IndexWriter writer)
      {
         // remove older index entry
         var id = entry.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("Text", entry.Text, Field.Store.YES, Field.Index.ANALYZED));
         /*doc.Add (new Field ("Term", entry.Item.Term.Name, Field.Store.YES, Field.Index.ANALYZED));
         doc.Add (new Field ("Source", entry.Item.Source.Id.ToString (CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));*/
         if (entry.Headers.Count > 0)
            doc.Add (new Field ("Header", entry.Headers.Select (h => h.Text).Aggregate ((i, j) => i + " " + j), Field.Store.YES, Field.Index.ANALYZED));
         if (entry.Footers.Count > 0)
            doc.Add (new Field ("Footer", entry.Footers.Select (f => f.Text).Aggregate ((i, j) => i + " " + j), Field.Store.YES, Field.Index.ANALYZED));
         /*if (entry.Item.Term.Tags.Count > 0)
            doc.Add (new Field ("Tags", entry.Item.Term.Tags.Select (t => t.Id.ToString (CultureInfo.InvariantCulture))
                  .Aggregate ((i, j) => i + " " + j), Field.Store.YES, Field.Index.NOT_ANALYZED));*/
         // add entry to index
         writer.AddDocument (doc);
      }

      private static void AddIndex (Writer write, IndexWriter writer)
      {
         // remove older index entry
         var id = write.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("Name", write.Name ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         doc.Add (new Field ("About", write.About ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         // add entry to index
         writer.AddDocument (doc);
      }

      private static void AddIndex (Tag tag, IndexWriter writer)
      {
         // remove older index entry
         var id = tag.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("Name", tag.Name ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         // add entry to index
         writer.AddDocument (doc);
      }

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
         doc.Add (new Field ("Name", book.Title ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         if (book.Tags.Count > 0)
         {
            doc.Add (new Field ("Tags", book.Tags.Select (t => t.Tag.Name)
                  .Aggregate ((i, j) => i + " " + j), Field.Store.YES, Field.Index.NOT_ANALYZED));
         }
         // add entry to index
         writer.AddDocument (doc);
      }

      private static void AddIndex (SeekDeepWithin.Pocos.Version version, IndexWriter writer)
      {
         // remove older index entry
         var id = version.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("Name", version.Title ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         // add entry to index
         writer.AddDocument (doc);
      }

      private static void AddIndex (VersionSubBook subBook, IndexWriter writer)
      {
         // remove older index entry
         var id = subBook.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("Name", subBook.SubBook.Name ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         doc.Add (new Field ("Hide", subBook.Hide.ToString(), Field.Store.YES, Field.Index.ANALYZED));
         // add entry to index
         writer.AddDocument (doc);
      }

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

      private static void AddIndex (GlossaryTerm term, IndexWriter writer)
      {
         // remove older index entry
         var id = term.Id.ToString (CultureInfo.InvariantCulture);
         var searchQuery = new TermQuery (new Term ("Id", id));
         writer.DeleteDocuments (searchQuery);
         // add new index entry
         var doc = new Document ();
         // add lucene fields mapped to db fields
         doc.Add (new Field ("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
         doc.Add (new Field ("Name", term.Name ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
         // add entry to index
         writer.AddDocument (doc);
      }

      /// <summary>
      /// Parses the query text.
      /// </summary>
      /// <param name="searchQuery">Text to search for.</param>
      /// <param name="parser">Query parser.</param>
      /// <param name="fuzzy"></param>
      /// <returns>The query with the text parsed.</returns>
      private static Query ParseQuery (string searchQuery, QueryParser parser, bool fuzzy)
      {
         //if (fuzzy) return new FuzzyQuery(new Term("Name", searchQuery), 0.5f);
         Query query;
         try
         {
            query = parser.Parse (searchQuery.Trim ());
         }
         catch (ParseException)
         {
            query = parser.Parse (QueryParser.Escape (searchQuery.Trim ()));
         }
         return query;
      }

      /// <summary>
      /// Gets the lucene directory for books.
      /// </summary>
      private static FSDirectory BookDirectory { get { return InitDirectory (ref s_BookDirectory, "book"); } }

      /// <summary>
      /// Gets the lucene directory for books.
      /// </summary>
      private static FSDirectory VersionDirectory { get { return InitDirectory (ref s_VersionDirectory, "version"); } }

      /// <summary>
      /// Gets the lucene directory for books.
      /// </summary>
      private static FSDirectory SubBookDirectory { get { return InitDirectory (ref s_SubBookDirectory, "subbook"); } }

      /// <summary>
      /// Gets the lucene directory for books.
      /// </summary>
      private static FSDirectory ChapterDirectory { get { return InitDirectory (ref s_ChapterDirectory, "chapter"); } }

      /// <summary>
      /// Gets the lucene directory for books.
      /// </summary>
      private static FSDirectory PassageDirectory { get { return InitDirectory (ref s_PassageDirectory, "passage"); } }

      /// <summary>
      /// Gets the lucene directory for glossary entries.
      /// </summary>
      private static FSDirectory TermDirectory { get { return InitDirectory (ref s_TermDirectory, "term"); } }

      /// <summary>
      /// Gets the lucene directory for glossary entries.
      /// </summary>
      private static FSDirectory GlossaryDirectory { get { return InitDirectory (ref s_GlossaryDirectory, "glossary"); } }

      /// <summary>
      /// Gets the lucene directory for writers.
      /// </summary>
      private static FSDirectory WriterDirectory { get { return InitDirectory (ref s_WriterDirectory, "writer"); } }

      /// <summary>
      /// Gets the lucene directory for tags.
      /// </summary>
      private static FSDirectory TagDirectory { get { return InitDirectory (ref s_TagDirectory, "tag"); } }

      /// <summary>
      /// Initializes the given directory.
      /// </summary>
      /// <param name="directory">Directory to initialize.</param>
      /// <param name="path">Sub path for directory.</param>
      /// <returns>The initialized directory.</returns>
      private static FSDirectory InitDirectory (ref FSDirectory directory, string path)
      {
         if (string.IsNullOrWhiteSpace (s_LuceneDir))
            s_LuceneDir = HostingEnvironment.MapPath (HttpRuntime.AppDomainAppVirtualPath) + "sdw_lucene";
         if (directory == null)
            directory = FSDirectory.Open (new DirectoryInfo (s_LuceneDir + "\\" + path));
         if (IndexWriter.IsLocked (directory))
            IndexWriter.Unlock (directory);
         var lockFilePath = Path.Combine (s_LuceneDir, path, "write.lock");
         if (System.IO.File.Exists (lockFilePath))
            System.IO.File.Delete (lockFilePath);
         return directory;
      }
   }
}