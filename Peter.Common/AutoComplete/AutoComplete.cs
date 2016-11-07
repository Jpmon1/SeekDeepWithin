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
using Peter.Common.Utilities;

namespace Peter.Common.AutoComplete
{
   /// <summary>
   /// Auto complete attached property.
   /// </summary>
   public static class AutoComplete
   {
      /// <summary>
      /// Attached Property for auto complete data provider.
      /// </summary>
      public static readonly DependencyProperty DataProviderProperty =
         DependencyProperty.RegisterAttached ("DataProvider", typeof (IAutoCompleteDataProvider), typeof (AutoComplete),
         new PropertyMetadata(null, DataProviderChanged));

      /// <summary>
      /// Gets the IAutoCompleteDataProvider from the given element.
      /// </summary>
      /// <param name="target">UI element to get IAutoCompleteDataProvider from.</param>
      /// <returns>IAutoCompleteDataProvider</returns>
      public static IAutoCompleteDataProvider GetDataProvider (UIElement target)
      {
         return (IAutoCompleteDataProvider)target.GetValue (DataProviderProperty);
      }

      /// <summary>
      /// Sets the IAutoCompleteDataProvider for the given UI element.
      /// </summary>
      /// <param name="target">UI element to set IAutoCompleteDataProvider.</param>
      /// <param name="value">IAutoCompleteDataProvider to use.</param>
      public static void SetDataProvider (UIElement target, IAutoCompleteDataProvider value)
      {
         target.SetValue (DataProviderProperty, value);
      }

      /// <summary>
      /// Dependency property to get autocomplete popup controller.
      /// </summary>
      internal static DependencyProperty PopupControllerProperty =
         DependencyProperty.RegisterAttached (
              "PopupController",
              typeof (PopupController),
              typeof (AutoComplete),
              new PropertyMetadata (null));

      /// <summary>
      /// Gets the autocomplete popup controller from the given element.
      /// </summary>
      /// <param name="target">UI element to get autocomplete popup controller from.</param>
      /// <returns>Autocomplete popup controller.</returns>
      internal static PopupController GetPopupController (UIElement target)
      {
         return (PopupController)target.GetValue (PopupControllerProperty);
      }

      /// <summary>
      /// Sets the autocomplete popup controller for the given UI element.
      /// </summary>
      /// <param name="target">UI element to set autocomplete popup controller.</param>
      /// <param name="value">Autocomplete popup controller to use.</param>
      internal static void SetPopupController (UIElement target, PopupController value)
      {
         target.SetValue (PopupControllerProperty, value);
      }

      /// <summary>
      /// Occurs when a data provider changes.
      /// </summary>
      /// <param name="d"></param>
      /// <param name="e"></param>
      private static void DataProviderChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var textBox = d as TextBox;
         if (textBox == null && d is ComboBox)
         {
            foreach (var child in d.GetVisualDescendents<TextBox> ())
            {
               if (child.Name == "PART_EditableTextBox")
               {
                  textBox = child;
                  break;
               }
            }
         }
         if (textBox == null) return;

         // Remove old controller...
         var controller = GetPopupController (textBox);
         if (controller != null) controller.Detach ();

         if (e.NewValue != null)
            SetPopupController (textBox, new PopupController (textBox));
      }
   }
}
