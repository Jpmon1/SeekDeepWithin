/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Collections.ObjectModel;

namespace Peter.Common.MainMenu
{
   /// <summary>
   /// The interface used for main menu headers.
   /// </summary>
   public interface IMainMenuHeader
   {
      /// <summary>
      /// Gets the title of the menu header.
      /// </summary>
      string Title { get; }

      /// <summary>
      /// Gets the tool tip of the menu item.
      /// </summary>
      string ToolTip { get; }

      /// <summary>
      /// Gets the Icon url of the menu item.
      /// </summary>
      MainMenuIcon Icon { get; }

      /// <summary>
      /// Gets the priority of the menu item.
      /// </summary>
      int Priority { get; }

      /// <summary>
      /// Gets or Sets if the header is selected.
      /// </summary>
      bool IsSelected { get; set; }

      /// <summary>
      /// Gets or Sets if the item is enabled.
      /// </summary>
      bool IsEnabled { get; set; }

      /// <summary>
      /// Gets or Sets if the menu item is visible.
      /// </summary>
      bool IsVisible { get; set; }

      /// <summary>
      /// Gets the list of children menu items.
      /// </summary>
      ObservableCollection<IMainMenuItem> Children { get; }

      /// <summary>
      /// Gets the list of children menu items for a drop down menu on the header.
      /// </summary>
      ObservableCollection <IMainMenuItem> MenuItems { get; }
   }
}
