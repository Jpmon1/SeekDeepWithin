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
   /// The amount of time for a status delay.
   /// </summary>
   public enum StatusTimeDelay
   {
      /// <summary>
      /// A short delay 2 sec.
      /// </summary>
      Short = 2,

      /// <summary>
      /// A medium delay 5 sec.
      /// </summary>
      Medium = 5,

      /// <summary>
      /// A long delay 10 sec.
      /// </summary>
      Long = 10,

      /// <summary>
      /// A super long delay 20 sec.
      /// </summary>
      SuperLong = 20,

      /// <summary>
      /// Indefinite amount of time, until something else comes along.
      /// </summary>
      Indefinite = 0
   }
}
