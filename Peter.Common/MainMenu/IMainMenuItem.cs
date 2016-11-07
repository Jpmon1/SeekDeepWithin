/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Peter.Common.MainMenu
{
   /// <summary>
   /// The interface use for main menu items.
   /// </summary>
   public interface IMainMenuItem
   {
      /// <summary>
      /// Gets the priority of the menu item.
      /// </summary>
      int Priority { get; }

      /// <summary>
      /// Gets if this is a header menu item.
      /// </summary>
      bool IsHeaderMenuItem { get; }

      /// <summary>
      /// Gets the of the menu item separated with pipes ('|').
      /// </summary>
      string HeaderName { get; }

      /// <summary>
      /// Gets the title of the menu item.
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
      /// Gets or Sets if the item is enabled.
      /// </summary>
      bool IsEnabled { get; set; }

      /// <summary>
      /// Gets a command parameter to pass with the command of the menu item.
      /// </summary>
      object CommandParameter { get; }

      /// <summary>
      /// Gets the command to invoke when running the menu item.
      /// </summary>
      ICommand Command { get; }

      /// <summary>
      /// Gets the list of children menu items.
      /// </summary>
      ObservableCollection<IMainMenuItem> Children { get; }

      /// <summary>
      /// Gets or Sets if the menu item is visible.
      /// </summary>
      bool IsVisible { get; set; }

      /// <summary>
      /// Gets or Sets the context the menu is opening in.
      /// </summary>
      object Context { get; set; }
   }
}
