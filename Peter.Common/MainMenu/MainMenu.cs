/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Peter.Common.Icons;

namespace Peter.Common.MainMenu
{
   /// <summary>
   /// A MVVM main menu control for an application.
   /// </summary>
   public class MainMenu : TabControl
   {
      #region Setup

      /// <summary>
      /// Static constructor.
      /// </summary>
      static MainMenu ()
      {
         DefaultStyleKeyProperty.OverrideMetadata (typeof (MainMenu),
            new FrameworkPropertyMetadata (typeof (MainMenu)));
      }

      /// <summary>
      /// Initializes a new main menu.
      /// </summary>
      public MainMenu ()
      {
         this.IconSize = IconSize.Medium;
         this.HeaderIconSize = IconSize.Small;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Dependency property for icon size.
      /// </summary>
      public static readonly DependencyProperty IconSizeProperty =
         DependencyProperty.Register ("IconSize", typeof (IconSize), typeof (MainMenu),
         new PropertyMetadata (default(IconSize)));

      /// <summary>
      /// Gets or Sets the icon size for the menu.
      /// </summary>
      public IconSize IconSize
      {
         get { return (IconSize) GetValue (IconSizeProperty); }
         set { SetValue (IconSizeProperty, value); }
      }

      /// <summary>
      /// Dependency property for header icon size.
      /// </summary>
      public static readonly DependencyProperty HeaderIconSizeProperty =
         DependencyProperty.Register ("HeaderIconSize", typeof (IconSize), typeof (MainMenu),
         new PropertyMetadata (default(IconSize)));

      /// <summary>
      /// Gets or Sets the icon size for the headers.
      /// </summary>
      public IconSize HeaderIconSize
      {
         get { return (IconSize) GetValue (HeaderIconSizeProperty); }
         set { SetValue (HeaderIconSizeProperty, value); }
      }

      /// <summary>
      /// Dependency property for the quick access menu items.
      /// </summary>
      public static DependencyProperty QuickAccessMenuItemsProperty =
         DependencyProperty.Register ("QuickAccessMenuItems", typeof (ObservableCollection <IMainMenuQuickAccessItem>), typeof (MainMenu),
         new PropertyMetadata (new ObservableCollection<IMainMenuQuickAccessItem> ()));

      /// <summary>
      /// Gets or Sets the list of quick access menu items.
      /// </summary>
      public ObservableCollection<IMainMenuQuickAccessItem> QuickAccessMenuItems
      {
         get { return (ObservableCollection<IMainMenuQuickAccessItem>)GetValue (QuickAccessMenuItemsProperty); }
         set { SetValue (QuickAccessMenuItemsProperty, value); }
      }

      #endregion

      #region Overrides

      /// <summary>
      /// Checks to see if the given item is the required container.
      /// </summary>
      /// <param name="item">Item to check.</param>
      /// <returns>True if item is dock content, otherwise false.</returns>
      protected override bool IsItemItsOwnContainerOverride (object item)
      {
         if (item == null)
            throw new InvalidOperationException ("Menu tab item is null!");
         return item is MainMenuHeader;
      }

      /// <summary>
      /// Gets the needed container for items in a dock site.
      /// </summary>
      /// <returns></returns>
      protected override DependencyObject GetContainerForItemOverride ()
      {
         return new MainMenuHeader { MainMenu = this };
      }

      #endregion
   }
}
