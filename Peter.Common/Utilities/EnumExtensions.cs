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
 * Adopted from: http://stackoverflow.com/a/417217
 **/

using System;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Extension methods for enumerations.
   /// </summary>
   public static class EnumExtensions
   {
      /// <summary>
      /// Checks to see if the Enumeration object has the given value.
      /// </summary>
      /// <typeparam name="T">Enumeration type.</typeparam>
      /// <param name="enumeration">The Enumeration object.</param>
      /// <param name="value">Value to check.</param>
      /// <returns>True if Enumeration object has the requested value, otherwise false..</returns>
      public static bool Has<T> (this Enum enumeration, T value)
      {
         try
         {
            return (((int)(object)enumeration & (int)(object)value) == (int)(object)value);
         }
         catch { return false; }
      }

      /// <summary>
      /// Checks to see if the Enumeration object and the given value are equal.
      /// </summary>
      /// <typeparam name="T">Enumeration type.</typeparam>
      /// <param name="enumeration">The Enumeration object.</param>
      /// <param name="value">Value to check.</param>
      /// <returns>True if Enumeration object is the requested value, otherwise false..</returns>
      public static bool Is<T> (this Enum enumeration, T value)
      {
         try
         {
            return (int)(object)enumeration == (int)(object)value;
         }
         catch { return false; }
      }

      /// <summary>
      /// Adds the given value to the enumeration.
      /// </summary>
      /// <typeparam name="T">Enumeration type.</typeparam>
      /// <param name="enumeration">The Enumeration object.</param>
      /// <param name="value">Value to check.</param>
      /// <returns>New enumeration object with value added.</returns>
      public static T Add<T> (this Enum enumeration, T value)
      {
         try
         {
            return (T)(object)(((int)(object)enumeration | (int)(object)value));
         }
         catch (Exception ex)
         {
            throw new ArgumentException (
                string.Format ("Could not append value from enumerated type '{0}'.", typeof (T).Name), ex);
         }
      }

      /// <summary>
      /// Removes the given value from the enumeration.
      /// </summary>
      /// <typeparam name="T">Enumeration type.</typeparam>
      /// <param name="enumeration">The Enumeration object.</param>
      /// <param name="value">Value to check.</param>
      /// <returns>New enumeration object with value removed.</returns>
      public static T Remove<T> (this Enum enumeration, T value)
      {
         try
         {
            return (T)(object)(((int)(object)enumeration & ~(int)(object)value));
         }
         catch (Exception ex)
         {
            throw new ArgumentException (
                string.Format ("Could not remove value from enumerated type '{0}'.", typeof (T).Name), ex);
         }
      }

   }
}
