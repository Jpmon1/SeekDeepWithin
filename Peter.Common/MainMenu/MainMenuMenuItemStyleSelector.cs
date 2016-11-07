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

namespace Peter.Common.MainMenu
{
   /// <summary>
   /// A style selector for the menus in the main menu.
   /// </summary>
   public class MainMenuMenuItemStyleSelector : StyleSelector
   {
      /// <summary>
      /// When overridden in a derived class, returns a <see cref="T:System.Windows.Style"/> based on custom logic.
      /// </summary>
      /// <returns>
      /// Returns an application-specific style to apply; otherwise, null.
      /// </returns>
      /// <param name="item">The content.</param>
      /// <param name="container">The element to which the style will be applied.</param>
      public override Style SelectStyle (object item, DependencyObject container)
      {
         if (item is MainMenuSeperator)
            return (Style)((FrameworkElement)container).FindResource ("SeparatorStyle");
         return (Style)((FrameworkElement)container).FindResource ("MainMenuDropDownItemStyle");
      }
   }
}
