using System.IO;
using System.Web;
using System.Web.Hosting;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace SeekDeepWithin.SdwSearch
{
   /// <summary>
   /// Implements lucene indexing and searching.
   /// http://www.lucenetutorial.com/lucene-query-syntax.html
   /// </summary>
   internal static class SearchCommon
   {
      private static string s_LuceneDir;
      internal const int HITS_LIMIT = 1000;

      /// <summary>
      /// Parses the query text.
      /// </summary>
      /// <param name="searchQuery">Text to search for.</param>
      /// <param name="parser">Query parser.</param>
      /// <returns>The query with the text parsed.</returns>
      internal static Query ParseQuery (string searchQuery, QueryParser parser)
      {
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
      /// Initializes the given directory.
      /// </summary>
      /// <param name="directory">Directory to initialize.</param>
      /// <param name="path">Sub path for directory.</param>
      /// <returns>The initialized directory.</returns>
      internal static FSDirectory InitDirectory (ref FSDirectory directory, string path)
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
         if (!directory.Directory.Exists)
            directory.Directory.Create();
         return directory;
      }
   }
}