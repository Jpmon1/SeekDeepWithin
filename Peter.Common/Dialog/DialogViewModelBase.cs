/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Windows.Input;

namespace Peter.Common.Dialog
{
   /// <summary>
   /// Base view mode for a dialog.
   /// </summary>
   public class DialogViewModelBase : ViewModelBase
   {
      #region Fields

      /// <summary>
      /// Dialog result.
      /// </summary>
      private bool? m_DialogResult;

      /// <summary>
      /// Ok Command.
      /// </summary>
      private RelayCommand m_CmdOk;

      /// <summary>
      /// Cancel Command.
      /// </summary>
      private RelayCommand m_CmdCancel;

      #endregion

      #region Properties

      /// <summary>
      /// Gets the ok command.
      /// </summary>
      public ICommand OkCommand
      {
         get { return this.m_CmdOk ?? (this.m_CmdOk = new RelayCommand (OkExecuted, CanOkExecute)); }
      }

      /// <summary>
      /// Gets the cancel command.
      /// </summary>
      public ICommand CancelCommand
      {
         get { return this.m_CmdCancel ?? (this.m_CmdCancel = new RelayCommand (CancelExecuted, CanCancelExecute)); }
      }

      /// <summary>
      /// Gets or Sets the dialog result to close any attached Model view.
      /// </summary>
      public bool? DialogResult
      {
         get { return this.m_DialogResult; }
         set
         {
            this.m_DialogResult = value;
            this.OnPropertyChanged ("DialogResult");
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Occurs when the ok command is executed.
      /// </summary>
      /// <param name="parameter">Command parameter.</param>
      protected virtual void OkExecuted (object parameter)
      {
         this.DialogResult = true;
      }

      /// <summary>
      /// Checks if ok can be clicked.
      /// </summary>
      /// <param name="parameter">Command parameter.</param>
      protected virtual bool CanOkExecute (object parameter)
      {
         return true;
      }

      /// <summary>
      /// Occurs when the cancel command is executed.
      /// </summary>
      /// <param name="parameter">Command parameter.</param>
      protected virtual void CancelExecuted (object parameter)
      {
         this.DialogResult = false;
      }

      /// <summary>
      /// Checks if cancel can be clicked.
      /// </summary>
      /// <param name="parameter">Command parameter.</param>
      protected virtual bool CanCancelExecute (object parameter)
      {
         return true;
      }

      #endregion
   }
}
