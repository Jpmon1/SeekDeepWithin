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

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Peter.Common.MainMenu
{
   /// <summary>
   /// Represents a seperator on the main menu.
   /// </summary>
   public sealed class MainMenuSeperator : IMainMenuItem
   {
      /// <summary>
      /// Gets the priority of the menu item.
      /// </summary>
      public int Priority { get; set; }

      /// <summary>
      /// Gets if this is a header menu item.
      /// </summary>
      public bool IsHeaderMenuItem { get; set; }

      /// <summary>
      /// Gets the of the menu item separated with pipes ('|').
      /// </summary>
      public string HeaderName { get; set; }

      /// <summary>
      /// Gets or Sets the context the menu is opening in.
      /// </summary>
      public object Context { get; set; }

      /// <summary>
      /// Gets the title of the menu item.
      /// </summary>
      public string Title
      {
         get { return "MainMenuSeperator"; }
      }

      /// <summary>
      /// Gets the tool tip of the menu item.
      /// </summary>
      public string ToolTip
      {
         get { return null; }
      }

      /// <summary>
      /// Gets the Icon url of the menu item.
      /// </summary>
      public MainMenuIcon Icon
      {
         get { return MainMenuIcon.None; }
      }

      /// <summary>
      /// Gets or Sets if the item is enabled.
      /// </summary>
      public bool IsEnabled
      {
         get { return false; }
         set { }
      }

      /// <summary>
      /// Gets a command parameter to pass with the command of the menu item.
      /// </summary>
      public object CommandParameter
      {
         get { return null; }
      }

      /// <summary>
      /// Gets the command to invoke when running the menu item.
      /// </summary>
      public ICommand Command
      {
         get { return null; }
      }

      /// <summary>
      /// Gets the list of children menu items.
      /// </summary>
      public ObservableCollection <IMainMenuItem> Children
      {
         get { return null; }
      }

      /// <summary>
      /// Gets or Sets if the menu item is visible.
      /// </summary>
      public bool IsVisible
      {
         get { return true; }
         set { }
      }
   }
}
