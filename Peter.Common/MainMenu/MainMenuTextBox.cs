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
   /// Represents a text box for the main menu.
   /// </summary>
   public class MainMenuTextBox : ViewModelBase, IMainMenuItem, IMainMenuQuickAccessItem
   {
      private int m_Width;
      private int m_Caret;
      private string m_Text;
      private string m_Title;
      private string m_ToolTip;
      private bool m_IsFocused;
      private ICommand m_Command;
      private ICommand m_KeyCommand;
      private ICommand m_FocusCommand;
      private bool m_IsEnabled = true;
      private bool m_IsVisible = true;
      private object m_CommandParameter;
      private MainMenuIcon m_Icon = MainMenuIcon.None;

      /// <summary>
      /// Event occurs when the text in the text box changes.
      /// </summary>
      public event TextEventHandler TextChanged;

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
      /// Gets or Sets the position of the caret.
      /// </summary>
      public int CaretPosition
      {
         get { return this.m_Caret; }
         set
         {
            this.m_Caret = value;
            this.OnPropertyChanged ("CaretPosition");
         }
      }

      /// <summary>
      /// Gets or Sets the placeholder text for the textbox.
      /// </summary>
      public string Title
      {
         get { return this.m_Title; }
         set
         {
            if (this.m_Title != value)
            {
               this.m_Title = value;
               this.OnPropertyChanged ("Title");
            }
         }
      }

      /// <summary>
      /// Gets or Sets if the text box is focused or not.
      /// Use with FocusExtension.
      /// </summary>
      public bool IsFocused
      {
         get { return this.m_IsFocused; }
         set
         {
            this.m_IsFocused = value;
            this.OnPropertyChanged ("IsFocused");
         }
      }

      /// <summary>
      /// Gets or sets the text in the text box.
      /// </summary>
      public string Text
      {
         get { return this.m_Text; }
         set
         {
            if (this.m_Text != value)
            {
               this.m_Text = value;
               this.OnPropertyChanged ("Text");
               if (this.TextChanged != null)
                  this.TextChanged (this, new TextEventArgs (this.m_Text));
            }
         }
      }

      /// <summary>
      /// Gets the url of the icon to display for the menu item.
      /// </summary>
      public MainMenuIcon Icon
      {
         get { return this.m_Icon; }
         set
         {
            if (this.m_Icon != value)
            {
               this.m_Icon = value;
               this.OnPropertyChanged ("Icon");
            }
         }
      }

      /// <summary>
      /// Gets or Sets if the item is enabled.
      /// </summary>
      public bool IsEnabled
      {
         get { return this.m_IsEnabled; }
         set
         {
            if (this.m_IsEnabled != value)
            {
               this.m_IsEnabled = value;
               this.OnPropertyChanged ("IsEnabled");
            }
         }
      }

      /// <summary>
      /// Gets the tool tip to display for the menu item.
      /// </summary>
      public string ToolTip
      {
         get { return this.m_ToolTip; }
         set
         {
            if (this.m_ToolTip != value)
            {
               this.m_ToolTip = value;
               this.OnPropertyChanged ("ToolTip");
            }
         }
      }

      /// <summary>
      /// Gets a command parameter for the menu item.
      /// </summary>
      public object CommandParameter
      {
         get { return this.m_CommandParameter; }
         set
         {
            if (this.m_CommandParameter != value)
            {
               this.m_CommandParameter = value;
               this.OnPropertyChanged ("CommandParameter");
            }
         }
      }

      /// <summary>
      /// Gets the action for the menu to perform when clicked.
      /// </summary>
      public ICommand Command
      {
         get { return this.m_Command; }
         set
         {
            if (this.m_Command != value)
            {
               this.m_Command = value;
               this.OnPropertyChanged ("Command");
            }
         }
      }

      /// <summary>
      /// Gets the command to call when a key is pressed.
      /// </summary>
      public ICommand KeyCommand
      {
         get { return this.m_KeyCommand; }
         set
         {
            if (this.m_KeyCommand != value)
            {
               this.m_KeyCommand = value;
               this.OnPropertyChanged ("KeyCommand");
            }
         }
      }

      /// <summary>
      /// Gets the action for when a focus event happens.
      /// </summary>
      public ICommand FocusCommand
      {
         get { return this.m_FocusCommand; }
         set
         {
            if (this.m_FocusCommand != value)
            {
               this.m_FocusCommand = value;
               this.OnPropertyChanged ("FocusCommand");
            }
         }
      }

      /// <summary>
      /// Gets or Sets if the menu item is visible.
      /// </summary>
      public bool IsVisible
      {
         get { return this.m_IsVisible; }
         set
         {
            m_IsVisible = value;
            this.OnPropertyChanged ("IsVisible");
         }
      }

      /// <summary>
      /// Gets the list of children menu items.
      /// </summary>
      public ObservableCollection<IMainMenuItem> Children
      {
         get { return null; }
      }

      /// <summary>
      /// Gets or Sets the desired width of the text box.
      /// </summary>
      public int Width
      {
         get { return this.m_Width; }
         set
         {
            this.m_Width = value;
            this.OnPropertyChanged ("Width");
         }
      }
   }
}
