/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System;
using System.Collections.Generic;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Helper methods for strings.
   /// </summary>
   public static class StringHelpers
   {
      /// <summary>
      /// Check if the provided string contains the given value ignoring case.
      /// </summary>
      /// <param name="text">String to check.</param>
      /// <param name="value">Value to check.</param>
      /// <returns>True if string contains the given value, otherwise false.</returns>
      public static bool ContainsIgnoreCase (this string text, string value)
      {
         return text.IndexOf (value, StringComparison.CurrentCultureIgnoreCase) >= 0;
      }

      /// <summary>
      /// Removes the given value from the string list disregarding the case.
      /// </summary>
      /// <param name="list">List to remove value from.</param>
      /// <param name="value">Value to remove.</param>
      public static void RemoveIgnoreCase (this IList <string> list, string value)
      {
         var lower = value.ToLower ();
         foreach (var str in list)
         {
            if (str.ToLower () == lower)
            {
               list.Remove (str);
               break;
            }
         }
      }
   }
}
