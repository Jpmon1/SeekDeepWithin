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
using Peter.Common.Icons;
using Peter.Common.Utilities;

namespace Peter.Common.MainMenu
{
   /// <summary>
   /// Represents an Item on the main menu.
   /// </summary>
   public class MainMenuItem : Control
   {
      private Button m_MainButton;
      private Button m_DropDownButton;

      /// <summary>
      /// Static constructor.
      /// </summary>
      static MainMenuItem ()
      {
         DefaultStyleKeyProperty.OverrideMetadata (typeof (MainMenuItem),
            new FrameworkPropertyMetadata (typeof (MainMenuItem)));
      }

      /// <summary>
      /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
      /// </summary>
      public override void OnApplyTemplate ()
      {
         base.OnApplyTemplate ();
         if (this.DataContext is MainMenuSeperator)
         {
            this.SetResourceReference (StyleProperty, "MainMenuSeparatorStyle");
         }
         else if (this.DataContext is MainMenuTextBox)
         {
            this.SetResourceReference (StyleProperty, "MainMenuTextBoxStyle");
         }
         else
         {
            this.m_MainButton = GetTemplateChild ("PART_Button") as Button;
            this.m_DropDownButton = GetTemplateChild ("PART_DropDown") as Button;
            if (this.m_MainButton != null)
            {
               if (this.m_MainButton.Command == null)
               {
                  if (this.m_DropDownButton != null)
                  {
                     this.m_DropDownButton.Visibility = Visibility.Collapsed;
                     this.m_MainButton.Click += this.OnDropDownClick;

                     var content = GetTemplateChild ("PART_MainButtonContent") as StackPanel;
                     if (content != null)
                     {
                        content.Margin = new Thickness(5, 0, 5, 0);
                        content.Children.Add (new IconDisplay
                                                 {
                                                    IconSize = IconSize.Smaller,
                                                    Icon = MainMenuIcon.ChevronDown,
                                                    Margin = new Thickness(4, 0, 0, 0)
                                                 });
                     }
                  }
               }
            }
            if (this.m_DropDownButton != null && this.m_DropDownButton.Visibility != Visibility.Collapsed)
            {
               this.m_DropDownButton.Click += this.OnDropDownClick;
            }

            var itemsControl = this.FindVisualParent <MainMenuItemsControl> ();
            if (itemsControl != null && itemsControl.Name == "PART_QuickAccess")
            {
               this.m_MainButton.SetResourceReference (StyleProperty, "MainMenuItemQaStyle");
               this.m_DropDownButton.SetResourceReference (StyleProperty, "MainMenuItemQaDropDownStyle");
            }
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
         this.m_DropDownButton.ContextMenu.PlacementTarget = this.m_MainButton ?? this.m_DropDownButton;
         this.m_DropDownButton.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
         this.m_DropDownButton.ContextMenu.IsOpen = true;
      }
   }
}
