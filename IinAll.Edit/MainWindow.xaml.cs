using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using IinAll.Edit.Logic;
using IinAll.Edit.Properties;

namespace IinAll.Edit
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow
   {
      private readonly MainViewModel m_ViewModel;

      /// <summary>
      /// Initializes the main window.
      /// </summary>
      public MainWindow ()
      {
         InitializeComponent ();
         this.DataContext = this.m_ViewModel = new MainViewModel ();
         this.Loaded += this.OnLoad;
         this.m_ViewModel.LoveAdded += this.OnLoveAdded;
      }

      /// <summary>
      /// Occurs when a love is added.
      /// </summary>
      /// <param name="sender">Main View Model.</param>
      /// <param name="e">Empty</param>
      private void OnLoveAdded (object sender, EventArgs e)
      {
         this.LoveScroller.ScrollToTop ();
      }

      /// <summary>
      /// Occurs when the window is loaded.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void OnLoad (object sender, RoutedEventArgs e)
      {
         this.Loaded -= this.OnLoad;
         this.m_ViewModel.Load ();
         this.PasswordBox.Password = this.m_ViewModel.Password;
      }

      /// <summary>
      /// Occurs when a key is pressed in the password box.
      /// </summary>
      /// <param name="sender">Password Box</param>
      /// <param name="e">KeyEventArgs</param>
      private void OnKeyPasswordKeyDown (object sender, KeyEventArgs e)
      {
         if ((e.Key == Key.Enter || e.Key == Key.Return) && this.m_ViewModel != null)
         {
            if (this.m_ViewModel.LoginCommand.CanExecute (null)) {
               this.m_ViewModel.LoginCommand.Execute (null);
               e.Handled = true;
            }
         }
      }

      /// <summary>
      /// OCcurs when the password changes.
      /// </summary>
      /// <param name="sender">Password Box</param>
      /// <param name="e">RoutedEventArgs</param>
      private void OnPasswordChanged (object sender, RoutedEventArgs e)
      {
         if (this.m_ViewModel != null) {
            this.m_ViewModel.Password = this.PasswordBox.Password;
         }
      }

      /// <summary>
      /// Raises the <see cref="E:System.Windows.Window.Closing" /> event.
      /// </summary>
      /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
      protected override void OnClosing (CancelEventArgs e)
      {
         base.OnClosing (e);
         Settings.Default.Save ();
         this.m_ViewModel.Save ();
      }
   }
}
