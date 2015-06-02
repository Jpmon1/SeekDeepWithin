using System.Web;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents parameters that can be passed to a search query.
   /// http://www.lucenetutorial.com/lucene-query-syntax.html
   /// </summary>
   public class SearchQueryViewModel
   {
      private string m_Decoded;

      /// <summary>
      /// Initializes a new search query view model.
      /// </summary>
      public SearchQueryViewModel ()
      {
         this.PageSize = 25;
      }

      /// <summary>
      /// Gets or Sets the query to search for.
      /// </summary>
      public string Query { get; set; }

      /// <summary>
      /// Gets the url decoded query to search for.
      /// </summary>
      public string QDecoded
      {
         get
         {
            if (string.IsNullOrWhiteSpace(this.m_Decoded))
               this.m_Decoded = HttpUtility.UrlDecode (this.Query);
            return this.m_Decoded;
         }
      }

      /// <summary>
      /// Gets or Sets the requested page to display.
      /// </summary>
      public int Page { get; set; }

      /// <summary>
      /// Gets or Sets the requested page to display.
      /// </summary>
      public int PageSize { get; set; }

      /// <summary>
      /// Gets or Sets the search type.
      /// 0 - Any Word
      /// 1 - All Words
      /// 2 - Phrase
      /// 3 - Custom
      /// </summary>
      public int SearchType { get; set; }

      /// <summary>
      /// Gets or Sets if we should show the search log or not.
      /// </summary>
      public bool ShowLog { get; set; }

      /// <summary>
      /// Gets or Sets if the search should be exact or not.
      /// </summary>
      public bool Exact { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching writers.
      /// </summary>
      public bool DoWriters { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching in headers.
      /// </summary>
      public bool DoHeaders { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching in footers.
      /// </summary>
      public bool DoFooters { get; set; }

      /// <summary>
      /// Gets or Sets the filter to use.
      /// </summary>
      public string Filter { get; set; }
   }
}