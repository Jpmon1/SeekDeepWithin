/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Windows;
using System.Windows.Controls;
using Peter.Common.Icons;

namespace Peter.Common.MainMenu
{
   /// <summary>
   /// Represents a main menu items control (the bar with all the buttons on it).
   /// </summary>
   public class MainMenuItemsControl : ItemsControl
   {
      /// <summary>
      /// Dependency property for the icon size.
      /// </summary>
      public static readonly DependencyProperty IconSizeProperty =
         DependencyProperty.Register ("IconSize", typeof (IconSize), typeof (MainMenuItemsControl),
         new PropertyMetadata (default(IconSize)));

      /// <summary>
      /// Gets or Sets the icon size for this menu item control.
      /// </summary>
      public IconSize IconSize
      {
         get { return (IconSize) GetValue (IconSizeProperty); }
         set { SetValue (IconSizeProperty, value); }
      }

      /// <summary>
      /// Determines if the specified item is (or is eligible to be) its own container.
      /// </summary>
      /// <param name="item">The item to check.</param>
      /// <returns>true if the item is (or is eligible to be) its own container; otherwise, false.</returns>
      protected override bool IsItemItsOwnContainerOverride (object item)
      {
         return item is MainMenuItem;
      }

      /// <summary>
      /// Creates or identifies the element that is used to display the given item.
      /// </summary>
      /// <returns>The element that is used to display the given item.</returns>
      protected override DependencyObject GetContainerForItemOverride ()
      {
         return new MainMenuItem ();
      }
   }
}
