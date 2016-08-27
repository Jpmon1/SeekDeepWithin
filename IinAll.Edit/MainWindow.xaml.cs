﻿using System.Windows;
using System.Windows.Input;
using IinAll.Edit.Logic;

namespace IinAll.Edit
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow
   {
      private readonly MainViewModel m_ViewModel;

      public MainWindow ()
      {
         InitializeComponent ();
         this.DataContext = this.m_ViewModel = new MainViewModel ();
      }

      private void OnKeyPasswordKeyDown (object sender, KeyEventArgs e)
      {
         /*if ((e.Key == Key.Enter || e.Key == Key.Return) && this.m_ViewModel != null)
         {
            if (this.m_ViewModel.LoginCommand.CanExecute(null))
               this.m_ViewModel.LoginCommand.Execute (null);
         }*/
      }

      private void OnPasswordChanged (object sender, RoutedEventArgs e)
      {
         if (this.m_ViewModel != null) {
            this.m_ViewModel.Password = this.PasswordBox.Password;
         }
      }
   }
}