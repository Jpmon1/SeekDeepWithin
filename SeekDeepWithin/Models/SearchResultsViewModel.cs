using System.Collections.ObjectModel;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View Model for searches.
   /// </summary>
   public class SearchResultsViewModel
   {
      private readonly Collection<SearchResult> m_Results = new Collection<SearchResult> ();

      /// <summary>
      /// Initializes a new search view model.
      /// </summary>
      public SearchResultsViewModel (SearchQueryViewModel query)
      {
         this.Query = query;
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
      /// Gets or Sets the total number of hits for this group.
      /// </summary>
      public int TotalHits { get; set; }

      /// <summary>
      /// Gets or Sets the starting result number of this group.
      /// </summary>
      public int Start { get; set; }

      /// <summary>
      /// Gets or Sets the edning result number of this group.
      /// </summary>
      public int End { get; set; }

      /// <summary>
      /// Gets or Sets the title of this group.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or Sets if we should show empty results set.
      /// </summary>
      public bool ShowEmpty { get; set; }

      /// <summary>
      /// Gets the collection of results.
      /// </summary>
      public Collection<SearchResult> Results { get { return this.m_Results; } }

      /// <summary>
      /// Adds a search result to the results.
      /// </summary>
      /// <param name="searchResult"></param>
      public void Add (SearchResult searchResult)
      {
         this.m_Results.Add (searchResult);
      }
   }
}