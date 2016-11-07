/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Peter.Common.MainMenu;

namespace Peter.Common.Icons
{
   /// <summary>
   /// Represents an Icon display control.
   /// </summary>
   public class IconDisplay : TextBlock
   {
      /// <summary>
      /// Initializes a new icon display object.
      /// </summary>
      public IconDisplay ()
      {
         this.VerticalAlignment = VerticalAlignment.Center;
         this.HorizontalAlignment = HorizontalAlignment.Center;
         this.FontFamily = new FontFamily (new Uri ("pack://application:,,,/Peter.Common;Component/"),
                                           "./Icons/#WebHostingHub-Glyphs");
      }

      /// <summary>
      /// Dependency property for the icon.
      /// </summary>
      public static readonly DependencyProperty IconProperty =
         DependencyProperty.Register ("Icon", typeof (MainMenuIcon), typeof (IconDisplay),
         new PropertyMetadata (default(MainMenuIcon), OnIconChanged));

      /// <summary>
      /// Gets or Sets the Icon to display in the box.
      /// </summary>
      public MainMenuIcon Icon
      {
         get { return (MainMenuIcon) GetValue (IconProperty); }
         set { SetValue (IconProperty, value); }
      }

      /// <summary>
      /// Dependency property for the icon's size.
      /// </summary>
      public static readonly DependencyProperty IconSizeProperty =
         DependencyProperty.Register ("IconSize", typeof (IconSize), typeof (IconDisplay),
         new PropertyMetadata (IconSize.Medium, OnIconSizeChanged));

      /// <summary>
      /// Gets or Sets the icon's size.
      /// </summary>
      public IconSize IconSize
      {
         get { return (IconSize) GetValue (IconSizeProperty); }
         set { SetValue (IconSizeProperty, value); }
      }

      /// <summary>
      /// Occurs when the icon of a display changes.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnIconChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var iconDisplay = d as IconDisplay;
         if (iconDisplay != null)
         {
            iconDisplay.Text = MainMenuIconConverter.GetIconCode ((MainMenuIcon)e.NewValue);
         }
      }

      /// <summary>
      /// Occurs when the icon size of a display changes.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnIconSizeChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var iconDisplay = d as IconDisplay;
         if (iconDisplay != null)
         {
            iconDisplay.FontSize = IconSizeConverter.GetIconSize ((IconSize)e.NewValue);
         }
      }
   }
}
