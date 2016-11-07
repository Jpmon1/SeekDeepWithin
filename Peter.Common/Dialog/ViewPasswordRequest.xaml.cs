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
using System.Windows.Input;

namespace Peter.Common.Dialog
{
   /// <summary>
   /// Interaction logic for PasswordRequestView.xaml
   /// </summary>
   public partial class ViewPasswordRequest
   {
      private ModelPasswordRequest m_ViewModel;

      /// <summary>
      /// Initializes a new password request view.
      /// </summary>
      public ViewPasswordRequest ()
      {
         InitializeComponent ();
         this.DataContextChanged += this.OnDataContextChange;
      }

      private void OnDataContextChange (object sender, DependencyPropertyChangedEventArgs e)
      {
         this.DataContextChanged -= this.OnDataContextChange;
         this.m_ViewModel = this.DataContext as ModelPasswordRequest;
      }

      /// <summary>
      /// Occurs when the password changes.
      /// </summary>
      /// <param name="sender">PasswordBox</param>
      /// <param name="e">RoutedEventArgs</param>
      private void OnPasswordChanged (object sender, RoutedEventArgs e)
      {
         if (this.m_ViewModel != null)
         {
            this.m_ViewModel.Password = this.PasswordBox.Password;
         }
      }

      private void OnKeyPasswordKeyDown (object sender, KeyEventArgs e)
      {
         if ((e.Key == Key.Enter || e.Key == Key.Return) && this.m_ViewModel != null)
         {
            this.m_ViewModel.OkExecuted (null);
         }
      }
   }
}
