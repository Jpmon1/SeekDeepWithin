using System.Collections.ObjectModel;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View Model for searches.
   /// </summary>
   public class SearchViewModel
   {
      private readonly Collection <SearchResult> m_Results;

      /// <summary>
      /// Initializes a new search view model.
      /// </summary>
      public SearchViewModel (SearchQueryViewModel query)
      {
         this.Query = query;
         this.m_Results = new Collection <SearchResult> ();
      }

      /// <summary>
      /// Gets the query searched for.
      /// </summary>
      public SearchQueryViewModel Query { get; private set; }

      /// <summary>
      /// Gets or Sets the parser log.
      /// </summary>
      public string ParserLog { get; set; }

      /// <summary>
      /// Gets the number of matches.
      /// </summary>
      public int NumMatches { get { return this.m_Results.Count; } }

      /// <summary>
      /// Gets the collection of results.
      /// </summary>
      public Collection<SearchResult> Results { get { return this.m_Results; } }
   }
}