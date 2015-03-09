using System.Collections.ObjectModel;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View Model for searches.
   /// </summary>
   public class SearchViewModel
   {
      private readonly Collection <PassageViewModel> m_Passages;

      /// <summary>
      /// Initializes a new search view model.
      /// </summary>
      public SearchViewModel ()
      {
         this.m_Passages = new Collection <PassageViewModel> ();
      }

      /// <summary>
      /// Gets or Sets the query searched for.
      /// </summary>
      public string Query { get; set; }

      /// <summary>
      /// Gets the collection of passages.
      /// </summary>
      public Collection<PassageViewModel> Passages { get { return this.m_Passages; } }

      /// <summary>
      /// Gets or Sets the parser log.
      /// </summary>
      public string ParserLog { get; set; }

      /// <summary>
      /// Gets or Sets if the parsing log should be shown.
      /// </summary>
      public bool ShowLog { get; set; }
   }
}