/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

namespace Peter.Common
{
   /// <summary>
   /// Represents the search options.
   /// </summary>
   public class SearchOptions
   {
      /// <summary>
      /// Initializes a new search options.
      /// </summary>
      public SearchOptions (SearchDirection direction, bool ignoreCase)
      {
         this.IgnoreCase = ignoreCase;
         this.Direction = direction;
      }

      /// <summary>
      /// Gets if the case should be ignored
      /// </summary>
      public SearchDirection Direction { get; private set; }

      /// <summary>
      /// Gets if the case should be ignored
      /// </summary>
      public bool IgnoreCase { get; private set; }
   }
}
