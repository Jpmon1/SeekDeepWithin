/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Windows.Controls;
using System.Windows.Input;
using Peter.Common.Utilities;

namespace Peter.Common.AutoComplete
{
   /// <summary>
   /// Interaction logic for AutoCompletePopup.xaml
   /// </summary>
   public partial class AutoCompletePopup
   {
      /// <summary>
      /// Initializes a new auto complete popup.
      /// </summary>
      public AutoCompletePopup ()
      {
         InitializeComponent ();
      }

      /// <summary>
      /// Occurs when the mouse moves over the popup.
      /// </summary>
      /// <param name="sender">Popup</param>
      /// <param name="e">MouseEventArgs</param>
      private void OnPopupMouseMove (object sender, MouseEventArgs e)
      {
         if (Mouse.Captured != null) return;
         var listBox = sender as ListBox;
         if (listBox != null)
         {
            var item = listBox.GetItemContainerAt (e.GetPosition (listBox)) as ListBoxItem;
            if (item != null)
            {
               item.IsSelected = true;
            }
         }
      }

      /// <summary>
      /// Occurs when the left mouse button is released over the popup.
      /// </summary>
      /// <param name="sender">Popup</param>
      /// <param name="e">MouseButtonEventArgs</param>
      private void OnPopupMouseLeftButtonUp (object sender, MouseButtonEventArgs e)
      {
         var listBox = sender as ListBox;
         if (listBox != null)
         {
            var item = listBox.GetItemContainerAt (e.GetPosition (listBox)) as ListBoxItem;
            if (item != null)
            {
               var viewModel = this.DataContext as PopupController;
               if (viewModel != null)
               {
                  viewModel.UpdateTextBox (item.Content.ToString ());
                  this.IsOpen = false;
               }
            }
         }
      }

      /// <summary>
      /// Occurs when the left mouse button is pressed over the popup.
      /// </summary>
      /// <param name="sender">Popup</param>
      /// <param name="e">MouseButtonEventArgs</param>
      private void OnPopupMouseLeftButtonDown (object sender, MouseButtonEventArgs e)
      {
         var listBox = sender as ListBox;
         if (listBox != null)
         {
            var item = listBox.GetItemContainerAt (e.GetPosition (listBox)) as ListBoxItem;
            if (item != null)
            {
               e.Handled = true;
            }
         }
      }
   }
}
