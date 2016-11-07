using System;
using System.Windows.Input;

namespace IinAll.Edit.Views
{
   /// <summary>
   /// Interaction logic for TruthView.xaml
   /// </summary>
   public partial class TruthView
   {
      public TruthView ()
      {
         InitializeComponent ();
      }

      private void OnKeyUp (object sender, KeyEventArgs e)
      {
         this.SetSelection ();
      }

      private void OnMouseUp (object sender, MouseButtonEventArgs e)
      {
         this.SetSelection ();
      }

      private void SetSelection ()
      {
         this.StartIndex.Text = this.Text.SelectionStart.ToString ();
         this.EndIndex.Text = " - " + Convert.ToString (this.Text.SelectionStart + this.Text.SelectionLength);
      }
   }
}
