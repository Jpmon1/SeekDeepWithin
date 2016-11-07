using System.Windows;
using System.Windows.Input;
using IinAll.Edit.Logic;

namespace IinAll.Edit.Views
{
   /// <summary>
   /// Interaction logic for LoveView.xaml
   /// </summary>
   public partial class LoveView
   {
      private LoveViewModel m_ViewModel;

      /// <summary>
      /// Initializes a new love view.
      /// </summary>
      public LoveView ()
      {
         InitializeComponent ();
         this.DataContextChanged += this.OnViewModelChanged;
      }

      /// <summary>
      /// Occurs when the data context changes.
      /// </summary>
      /// <param name="sender">This control</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private void OnViewModelChanged (object sender, DependencyPropertyChangedEventArgs e)
      {
         this.m_ViewModel = this.DataContext as LoveViewModel;
         if (this.m_ViewModel != null)
            this.DataContextChanged -= this.OnViewModelChanged;
      }

      private void OnKeyUp (object sender, KeyEventArgs e)
      {
         this.SetSelection ();
      }

      private void OnMouseUp (object sender, MouseButtonEventArgs e)
      {
         this.SetSelection ();
      }

      /// <summary>
      /// Sets the selection.
      /// </summary>
      private void SetSelection ()
      {
         if (this.m_ViewModel != null) {
            this.m_ViewModel.StartIndex = this.Text.SelectionStart;
            this.m_ViewModel.EndIndex = this.Text.SelectionStart + this.Text.SelectionLength;
         }
      }
   }
}
