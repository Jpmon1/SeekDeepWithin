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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peter.Common.Controls
{
   /// <summary>
   /// Attached properties
   /// </summary>
   public class Ex : DependencyObject
   {
      /// <summary>
      /// Attahced property for an execute command.
      /// </summary>
      public static readonly DependencyProperty ExecuteCommandProperty = DependencyProperty.RegisterAttached (
         "ExecuteCommand", typeof (ICommand), typeof (Ex),
         new PropertyMetadata (default(ICommand), OnExecuteCommandChanged));

      /// <summary>
      /// Occurs when the execute command value changes.
      /// </summary>
      /// <param name="d">Object command changed for.</param>
      /// <param name="e">Change arguments.</param>
      private static void OnExecuteCommandChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var control = d as Control;
         if (control == null) return;
         control.MouseDoubleClick += OnMouseDoubleClick;
         control.PreviewKeyDown += OnKeyDown;
      }

      /// <summary>
      /// Occurs when a key is pressed on the control.
      /// </summary>
      /// <param name="sender">Control key was pressed on.</param>
      /// <param name="e">Key event arguments.</param>
      private static void OnKeyDown (object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
            TryExecute (sender);
      }

      /// <summary>
      /// Occurs when a control has been double clicked;
      /// </summary>
      /// <param name="sender">Control that has been double clicked.</param>
      /// <param name="e">Mouse event arguments.</param>
      private static void OnMouseDoubleClick (object sender, MouseButtonEventArgs e)
      {
         TryExecute (sender);
      }

      private static void TryExecute (object sender)
      {
         var control = sender as DependencyObject;
         if (control == null) return;
         var execute = GetExecuteCommand (control);
         if (execute.CanExecute (null))
            execute.Execute (null);
      }

      /// <summary>
      /// Sets the execute command.
      /// </summary>
      /// <param name="element">Object to set command for.</param>
      /// <param name="value">The command to set.</param>
      public static void SetExecuteCommand (DependencyObject element, ICommand value)
      {
         element.SetValue (ExecuteCommandProperty, value);
      }

      /// <summary>
      /// Gets the execute command.
      /// </summary>
      /// <param name="element">Object to get command for.</param>
      /// <returns>The requested command.</returns>
      public static ICommand GetExecuteCommand (DependencyObject element)
      {
         return (ICommand) element.GetValue (ExecuteCommandProperty);
      }
   }
}
