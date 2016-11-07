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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Peter.Common.AutoComplete
{
   /// <summary>
   /// Class used to control pop ups for text boxes.
   /// </summary>
   internal class PopupController : INotifyPropertyChanged
   {
      /// <summary>
      /// Event used to notify when a property has changed.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

      private AutoCompletePopup m_Popup;
      private readonly TextBox m_AttachedTextBox;
      private IAutoCompleteDataProvider m_Provider;

      private bool m_Updating;
      private int m_SelectedIndex;
      private string m_SelectedListItem;

      /// <summary>
      /// Initializes a new popup controller object.
      /// </summary>
      /// <param name="textBox">Text box to attach popup to.</param>
      public PopupController (TextBox textBox)
      {
         this.ListItems = new ObservableCollection <string> ();
         this.m_AttachedTextBox = textBox;
         var window = Window.GetWindow (textBox);
         if (window != null)
         {
            if (window.IsLoaded)
               this.Initialize ();
            else
               window.Loaded += OnWindowOnLoaded;
            window.LocationChanged += OnWindowOnLocationChanged;
         }
      }

      /// <summary>
      /// Getst the list of items in the auto complete box.
      /// </summary>
      public ObservableCollection<string> ListItems { get; private set; }

      /// <summary>
      /// Gets or Sets the selected item in the pop up's list box.
      /// </summary>
      public string SelectedListItem
      {
         get { return this.m_SelectedListItem; }
         set
         {
            if (this.m_SelectedListItem != value)
            {
               this.m_SelectedListItem = value;
               this.OnPropertyChanged ("SelectedListItem");
            }
         }
      }

      /// <summary>
      /// Gets or Sets the selected index of the pop up's list box.
      /// </summary>
      public int SelectedIndex
      {
         get { return this.m_SelectedIndex; }
         set
         {
            if (this.m_SelectedIndex != value)
            {
               if (value < 0)
                  this.m_SelectedIndex = this.ListItems.Count - 1;
               else if (value >= this.ListItems.Count)
                  this.m_SelectedIndex = 0;
               else
                  this.m_SelectedIndex = value;
               this.OnPropertyChanged ("SelectedIndex");
            }
         }
      }

      /// <summary>
      /// Detaches controller from text box.
      /// </summary>
      public void Detach ()
      {
         var window = Window.GetWindow (this.m_AttachedTextBox);
         if (window != null)
         {
            window.LocationChanged -= OnWindowOnLocationChanged;
            window.Deactivated -= OnWindowOnLocationChanged;
            this.m_AttachedTextBox.TextChanged -= TextBoxTextChanged;
         }
      }

      /// <summary>
      /// Initializes the popup.
      /// </summary>
      private void Initialize ()
      {
         this.m_Popup = new AutoCompletePopup
            {
               Width = this.m_AttachedTextBox.Width,
               DataContext = this,
               Placement = PlacementMode.Bottom,
               PlacementTarget = this.m_AttachedTextBox
            };
         this.m_Provider = AutoComplete.GetDataProvider (this.m_AttachedTextBox);
         if (this.m_Provider == null)
            throw new InvalidOperationException ("Data provider for auto complete must not be null!");

         this.m_AttachedTextBox.TextChanged += TextBoxTextChanged;
         this.m_AttachedTextBox.PreviewKeyDown += TextBoxKeyDown;
         this.m_AttachedTextBox.LostFocus += TextBoxLostFocus;
      }

      /// <summary>
      /// Occurs when the attached text loses focus.
      /// </summary>
      /// <param name="sender">Attached text box.</param>
      /// <param name="e">RoutedEventArgs.</param>
      private void TextBoxLostFocus (object sender, RoutedEventArgs e)
      {
         this.m_Popup.IsOpen = false;
      }

      /// <summary>
      /// Occurs when the attached text box's text changes.
      /// </summary>
      /// <param name="sender">Attached text box.</param>
      /// <param name="e">TextChangedEventArgs.</param>
      private void TextBoxTextChanged (object sender, TextChangedEventArgs e)
      {
         this.ListItems.Clear ();
         if (m_Updating) return;
         string text = this.m_AttachedTextBox.Text;
         if (string.IsNullOrEmpty (text))
         {
            this.m_Popup.IsOpen = false;
            return;
         }
         foreach (var item in this.m_Provider.GetAutoCompleteItems (text))
            this.ListItems.Add (item);

         int itemCount = this.ListItems.Count;
         if (itemCount > 0)
         {
            this.m_Popup.Height = (itemCount > 10) ? 200 : itemCount * 20;
            this.m_Popup.IsOpen = true;
         }
         else
            this.m_Popup.IsOpen = false;
      }

      /// <summary>
      /// Occurs when a key is pressed in the attached text box.
      /// </summary>
      /// <param name="sender">Attached text box.</param>
      /// <param name="e">KeyEventArgs.</param>
      private void TextBoxKeyDown (object sender, KeyEventArgs e)
      {
         if (!this.m_Popup.IsOpen) return;
         if (e.Key == Key.Escape)
         {
            this.m_Popup.IsOpen = false;
            e.Handled = true;
            return;
         }

         if (e.Key == Key.Enter)
         {
            if (this.m_Popup.IsOpen && !string.IsNullOrEmpty (this.SelectedListItem))
            {
               e.Handled = true;
               this.m_Popup.IsOpen = false;
               this.UpdateTextBox (this.SelectedListItem);
            }
         }
         else if (e.Key == Key.Up)
            this.SelectedIndex--;
         else if (e.Key == Key.Down)
            this.SelectedIndex++;
      }

      /// <summary>
      /// Updates the text in the attached text box.
      /// </summary>
      /// <param name="text">Text to set.</param>
      internal void UpdateTextBox (string text)
      {
         this.m_Updating = true;
         this.m_AttachedTextBox.Text = text;
         this.m_AttachedTextBox.SelectAll ();
         this.m_Updating = false;
      }

      /// <summary>
      /// Occurs when the main window's location changes.
      /// </summary>
      /// <param name="sender">Main window</param>
      /// <param name="e">EventArgs</param>
      private void OnWindowOnLocationChanged (object sender, EventArgs e)
      {
         this.m_Popup.IsOpen = false;
      }

      /// <summary>
      /// Occurs when the main window is loaded.
      /// </summary>
      /// <param name="sender">Main window.</param>
      /// <param name="e">RoutedEventArgs</param>
      private void OnWindowOnLoaded (object sender, RoutedEventArgs e)
      {
         this.Initialize ();
         // Remove event...
         var window = sender as Window;
         if (window != null)
         {
            window.Loaded -= OnWindowOnLoaded;
            window.Deactivated += OnWindowOnLocationChanged;
            window.PreviewMouseDown += OnWindowMouseDown;
         }
      }

      /// <summary>
      /// Occurs on the window mouse down event.
      /// </summary>
      /// <param name="sender">Main window.</param>
      /// <param name="e">MouseButtonEventArgs</param>
      private void OnWindowMouseDown (object sender, MouseButtonEventArgs e)
      {
         if (!Equals (e.Source, this.m_AttachedTextBox))
            this.m_Popup.IsOpen = false;
      }

      /// <summary>
      /// Raises the property changed event.
      /// </summary>
      /// <param name="propertyName">Name of property that has changed.</param>
      private void OnPropertyChanged (string propertyName)
      {
         if (this.PropertyChanged != null)
            this.PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
      }
   }
}
