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
using System.Windows.Input;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Attached property for focusing a ui object.
   /// http://stackoverflow.com/a/1356781
   /// </summary>
   public static class FocusExtension
   {
      /// <summary>
      /// Gets if the object IsFocused.
      /// </summary>
      /// <param name="obj">Object to check for focus.</param>
      /// <returns>True if IsFocused is true, otherwise false.</returns>
      public static bool GetIsFocused (DependencyObject obj)
      {
         return (bool)obj.GetValue (IsFocusedProperty);
      }

      /// <summary>
      /// Sets if the object IsFocused.
      /// </summary>
      /// <param name="obj">Object to set focus.</param>
      /// <param name="value">True to be focused, otherwise false.</param>
      public static void SetIsFocused (DependencyObject obj, bool value)
      {
         obj.SetValue (IsFocusedProperty, value);
      }

      /// <summary>
      /// Attached dependency property to assign focusing.
      /// </summary>
      public static readonly DependencyProperty IsFocusedProperty =
         DependencyProperty.RegisterAttached ("IsFocused", typeof (bool), typeof (FocusExtension),
         new UIPropertyMetadata (false, OnIsFocusedPropertyChanged));

      /// <summary>
      /// Occurs when the attached IsFocused property changes.
      /// </summary>
      /// <param name="d">Object attached property is changing on.</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnIsFocusedPropertyChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var uie = (UIElement)d;
         if ((bool)e.NewValue)
         {
            uie.Focus ();
         }
      }

      /// <summary>
      /// Dependency property for focused events.
      /// </summary>
      public static readonly DependencyProperty FocusEventsProperty = 
         DependencyProperty.RegisterAttached ("FocusEvents", typeof (ICommand), typeof (FocusExtension),
         new UIPropertyMetadata (FocusEventsChanged));

      /// <summary>
      /// Sets the foucs events.
      /// </summary>
      /// <param name="element">DependencyObject</param>
      /// <param name="value">ICommand to execute when a focus event occurs.</param>
      public static void SetFocusEvents (DependencyObject element, ICommand value)
      {
         element.SetValue (FocusEventsProperty, value);
      }

      /// <summary>
      /// Gets the focus events.
      /// </summary>
      /// <param name="element">DependencyObject</param>
      /// <returns>The attached command to execute when a focus event occurs.</returns>
      public static ICommand GetFocusEvents (DependencyObject element)
      {
         return (ICommand) element.GetValue (FocusEventsProperty);
      }

      /// <summary>
      /// Occurs when the focus events have changed.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void FocusEventsChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var control = d as UIElement;
         if (control != null)
         {
            if (e.OldValue != null)
            {
               control.GotFocus -= OnGotFocus;
               control.LostFocus -= OnLostFocus;
            }
            if (e.NewValue != null)
            {
               control.GotFocus += OnGotFocus;
               control.LostFocus += OnLostFocus;
            }
         }
      }

      /// <summary>
      /// Occurs when a control as lost focus.
      /// </summary>
      /// <param name="sender">Control that lost focus.</param>
      /// <param name="e">RoutedEventArgs</param>
      private static void OnLostFocus (object sender, RoutedEventArgs e)
      {
         var command = GetFocusEvents (sender as UIElement);
         if (command.CanExecute (FocusEvent.LostFocus))
            command.Execute (FocusEvent.LostFocus);
      }

      /// <summary>
      /// Occurs when a control has received focus.
      /// </summary>
      /// <param name="sender">Control that received focus.</param>
      /// <param name="e">RoutedEventArgs</param>
      private static void OnGotFocus (object sender, RoutedEventArgs e)
      {
         var command = GetFocusEvents (sender as UIElement);
         if (command.CanExecute (FocusEvent.GotFocus))
            command.Execute (FocusEvent.GotFocus);
      }

      /// <summary>
      /// Types of focus events.
      /// </summary>
      public enum FocusEvent
      {
         /// <summary>
         /// Signifies that focus was received.
         /// </summary>
         GotFocus,

         /// <summary>
         /// Signifies that focus was lost.
         /// </summary>
         LostFocus
      }
   }
}
