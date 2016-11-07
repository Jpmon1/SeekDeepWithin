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

namespace Peter.Common
{
   /// <summary>
   /// Event handler for text event arguments.
   /// </summary>
   /// <param name="sender">Object that caused the event.</param>
   /// <param name="args">TextEventArgs</param>
   public delegate void TextEventHandler (object sender, TextEventArgs args);

   /// <summary>
   /// Basic text event arguments.
   /// </summary>
   public class TextEventArgs : EventArgs
   {
      /// <summary>
      /// Initializes a new text event argument.
      /// </summary>
      /// <param name="text">Text that raised the event.</param>
      public TextEventArgs (string text)
      {
         this.Text = text;
      }

      /// <summary>
      /// Gets the text that raised the event.
      /// </summary>
      public string Text { get; private set; }
   }
}
