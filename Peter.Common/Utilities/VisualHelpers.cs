/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Extension methods to help with visual objects.
   /// </summary>
   public static class VisualHelpers
   {
      /// <summary>
      /// Gets the real mouse position for the given visual.
      /// http://www.switchonthecode.com/tutorials/wpf-snippet-reliably-getting-the-mouse-position
      /// </summary>
      /// <param name="relativeTo">Visual to get mouse position from.</param>
      /// <returns>Correct mouse position.</returns>
      public static Point RealMousePosition (this Visual relativeTo)
      {
         if (relativeTo == null) return new Point();
         var w32Mouse = new Win32Point ();
         GetCursorPos (ref w32Mouse);
         return relativeTo.PointFromScreen (new Point (w32Mouse.X, w32Mouse.Y));
      }

      [StructLayout (LayoutKind.Sequential)]
      private struct Win32Point
      {
         public readonly Int32 X;
         public readonly Int32 Y;
      };

      [DllImport ("user32.dll")]
      [return: MarshalAs (UnmanagedType.Bool)]
      private static extern bool GetCursorPos (ref Win32Point pt);
   }
}
