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

using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Peter.Common.MainMenu
{
   /// <summary>
   /// Represents a tab item in the main menu.
   /// </summary>
   public class MainMenuHeader : TabItem
   {
      private TextBlock m_TitleBlock;
      private Button m_DropDownButton;

      /// <summary>
      /// Gets the main menu this tab is attached to.
      /// </summary>
      public MainMenu MainMenu { get; internal set; }

      /// <summary>
      /// Dependency property for the icon.
      /// </summary>
      public static readonly DependencyProperty IconProperty =
         DependencyProperty.Register ("Icon", typeof (MainMenuIcon), typeof (MainMenuHeader),
         new PropertyMetadata (MainMenuIcon.None));

      /// <summary>
      /// Gets or Sets the icon. This is a dependency property.
      /// </summary>
      public MainMenuIcon Icon
      {
         get { return (MainMenuIcon)GetValue (IconProperty); }
         set { SetValue (IconProperty, value); }
      }

      /// <summary>
      /// Dependency property for menu item tab hidden with inactive.
      /// </summary>
      public static readonly DependencyProperty HideOnInactiveProperty =
         DependencyProperty.Register ("HideOnInactive", typeof (bool), typeof (MainMenuHeader),
         new PropertyMetadata (default (bool)));

      /// <summary>
      /// Gets or Sets if we should hide the tab when it is inactive. This is a dependency property.
      /// </summary>
      public bool HideOnInactive
      {
         get { return (bool)GetValue (HideOnInactiveProperty); }
         set { SetValue (HideOnInactiveProperty, value); }
      }

      /// <summary>
      /// Dependency property for the menu items.
      /// </summary>
      public static readonly DependencyProperty MenuItemsProperty =
         DependencyProperty.Register ("MenuItems", typeof (IEnumerable), typeof (MainMenuHeader),
         new PropertyMetadata (default (IEnumerable)));

      /// <summary>
      /// Gets or Sets the menu items for the header.
      /// </summary>
      public IEnumerable MenuItems
      {
         get { return (IEnumerable)GetValue (MenuItemsProperty); }
         set { SetValue (MenuItemsProperty, value); }
      }

      /// <summary>
      /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
      /// </summary>
      public override void OnApplyTemplate ()
      {
         base.OnApplyTemplate ();

         // We attempt to get the context menu items...
         this.m_TitleBlock = GetTemplateChild ("PART_Title") as TextBlock;
         this.m_DropDownButton = GetTemplateChild ("PART_DropDown") as Button;
         if (this.m_DropDownButton != null)
         {
            this.m_DropDownButton.Click += this.OnDropDownClick;
         }
      }

      /// <summary>
      /// Occurs when the user clicks the drop down button.
      /// </summary>
      /// <param name="sender">The drop down button.</param>
      /// <param name="e">RoutedEventArgs</param>
      private void OnDropDownClick (object sender, RoutedEventArgs e)
      {
         this.m_DropDownButton.ContextMenu.IsEnabled = true;
         if (this.m_TitleBlock == null)
            this.m_DropDownButton.ContextMenu.PlacementTarget = this.m_DropDownButton;
         else
            this.m_DropDownButton.ContextMenu.PlacementTarget = this.m_TitleBlock;
         this.m_DropDownButton.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
         this.m_DropDownButton.ContextMenu.IsOpen = true;
      }
   }
}
