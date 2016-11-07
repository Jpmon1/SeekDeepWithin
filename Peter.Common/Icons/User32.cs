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

using System;
using System.Runtime.InteropServices;

namespace Peter.Common.Icons
{
   ///<summary>Provides exports from User32.dll</summary>
   public class User32
   {
      /// <summary>
      /// Provides access to function required to delete handle. This method is used internally
      /// and is not required to be called separately.
      /// </summary>
      /// <param name="hIcon">Pointer to icon handle.</param>
      /// <returns>N/A</returns>
      [DllImport ("User32.dll")]
      public static extern int DestroyIcon (IntPtr hIcon);
   }
}