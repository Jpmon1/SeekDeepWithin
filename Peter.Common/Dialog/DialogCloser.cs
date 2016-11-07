/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Windows;

namespace Peter.Common.Dialog
{
   /// <summary>
   /// Class used to close dialogs with pure MVVM.
   /// </summary>
   public static class DialogCloser
   {
      /// <summary>
      /// Dependency property for closing window witl MVVM.
      /// </summary>
      public static readonly DependencyProperty DialogResultProperty =
         DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), 
         typeof(DialogCloser), new PropertyMetadata(DialogResultChanged));

      /// <summary>
      /// Occurs when the dialog result changes.
      /// </summary>
      /// <param name="d">Window as DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void DialogResultChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var wndWindow = d as Window;
         bool bIsModal = System.Windows.Interop.ComponentDispatcher.IsThreadModal;
         if (wndWindow == null) return;
         wndWindow.DialogResult = e.NewValue as bool?;
         if (!bIsModal) wndWindow.Close();
      }

      /// <summary>
      /// Sets the dialog result property.
      /// </summary>
      /// <param name="window">Window setting property.</param>
      /// <param name="value">DialogResult value.</param>
      public static void SetDialogResult (DependencyObject window, bool? value)
      {
         window.SetValue(DialogResultProperty, value);
      }
   }
}
