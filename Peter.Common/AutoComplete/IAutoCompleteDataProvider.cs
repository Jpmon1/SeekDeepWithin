/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 *
 *  This code is provided on an AS IS basis, with no WARRANTIES,
 *  CONDITIONS or GUARANTEES of any kind.
 *
 **/
using System.Collections.Generic;

namespace Peter.Common.AutoComplete
{
   /// <summary>
   /// Interface for auto complete data provider.
   /// </summary>
   public interface IAutoCompleteDataProvider
   {
      /// <summary>
      /// Gets the list of auto complete items.
      /// </summary>
      /// <param name="filter">Text filter to apply.</param>
      /// <returns>Collection of auto complete items.</returns>
      IEnumerable<string> GetAutoCompleteItems (string filter);
   }
}