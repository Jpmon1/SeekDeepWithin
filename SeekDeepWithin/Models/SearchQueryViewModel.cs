using System.Web;
using SeekDeepWithin.Controllers;

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
         this.DoGlossary = true;
         this.DoPassages = true;
      }

      /// <summary>
      /// Gets or Sets the query to search for.
      /// </summary>
      public string Query { get; set; }

      /// <summary>
      /// Gets the lucene query to search for.
      /// </summary>
      public string Q { get; private set; }

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
      /// Gets or Sets if we are searching passages.
      /// </summary>
      public bool DoPassages { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching the glossary.
      /// </summary>
      public bool DoGlossary { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching tags.
      /// </summary>
      public bool DoTags { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching writers.
      /// </summary>
      public bool DoWriters { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching in passage headers.
      /// </summary>
      public bool DoPassHeaders { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching in passage footers.
      /// </summary>
      public bool DoPassFooters { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching in glossary headers.
      /// </summary>
      public bool DoGlossHeaders { get; set; }

      /// <summary>
      /// Gets or Sets if we are searching in glossary footers.
      /// </summary>
      public bool DoGlossFooters { get; set; }

      /// <summary>
      /// Gets or Sets the filter to use on books.
      /// </summary>
      public string BookFilter { get; set; }

      /// <summary>
      /// Gets or Sets the filter to use on the glossary.
      /// </summary>
      public string GlossaryFilter { get; set; }

      /// <summary>
      /// Builds the query to send to lucene.
      /// </summary>
      public void BuildQuery ()
      {
         if (this.SearchType == 0 || this.SearchType == 1)
         {
            var query = string.Empty;
            var words = this.QDecoded.GetWords ();
            foreach (var word in words)
            {
               if (!string.IsNullOrEmpty (query))
                  query += SearchType == 0 ? " OR " : " AND ";
               query += this.Exact ? word : word + "*";
            }
            this.Q = query;
         }
         else if (this.SearchType == 2)
         {
            this.Q = "\"" + this.QDecoded + "\"";
         }
         else
         {
            this.Q = this.QDecoded;
         }
      }
   }
}