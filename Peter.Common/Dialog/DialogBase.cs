/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Windows.Input;
using System.Windows.Threading;

namespace Peter.Common.Dialog
{
   /// <summary>
   /// Represents the base class for all dialogs.
   /// </summary>
   public class DialogBase : ViewModelBase
   {
      /// <summary>
      /// Event handler for dialog events.
      /// </summary>
      /// <param name="dialog"></param>
      public delegate void DialogBaseHandler (DialogBase dialog);

      /// <summary>
      /// Event occurs when the dialog is closing.
      /// </summary>
      public event DialogBaseHandler Closing;

      private double m_Width;
      private double m_Height;
      private bool m_ShowOkButton;
      private bool m_ShowYesButton;
      private bool m_ShowNoButton;
      private bool m_ShowCancelButton;
      private string m_Title;
      private string m_TxtError;
      private string m_TxtOkButton;
      private string m_TxtYesButton;
      private string m_TxtNoButton;
      private string m_TxtCancelButton;
      private bool m_IsVisible;
      private RelayCommand m_CmdDialogOk;
      private RelayCommand m_CmdDialogYes;
      private RelayCommand m_CmdDialogNo;
      private RelayCommand m_CmdDialogCancel;
      private DialogCloseAction m_DialogAction;

      /// <summary>
      /// Initializes a dialog base object.
      /// </summary>
      public DialogBase ()
      {
         this.OkButtonText = "Ok";
         this.YesButtonText = "Yes";
         this.NoButtonText = "No";
         this.CancelButtonText = "Cancel";
         this.ShowOkButton = true;
         this.ShowCancelButton = true;
      }

      /// <summary>
      /// Gets or Sets the title of the dialog.
      /// </summary>
      public string Title
      {
         get { return this.m_Title; }
         set
         {
            this.m_Title = value;
            this.OnPropertyChanged ("Title");
         }
      }

      /// <summary>
      /// Gets or Sets any error text.
      /// </summary>
      public string ErrorText
      {
         get { return this.m_TxtError; }
         set
         {
            this.m_TxtError = value;
            this.OnPropertyChanged ("ErrorText");
         }
      }

      /// <summary>
      /// Gets or sets the dialog visibility.
      /// </summary>
      public bool IsVisible
      {
         get { return this.m_IsVisible; }
         set
         {
            if (!value)
               this.RaiseClosingEvent ();
            this.m_IsVisible = value;
            this.OnPropertyChanged ("IsVisible");
         }
      }

      /// <summary>
      /// Gets or Sets the text on the ok button.
      /// </summary>
      public string OkButtonText
      {
         get { return this.m_TxtOkButton; }
         set
         {
            this.m_TxtOkButton = value;
            this.OnPropertyChanged ("OkButtonText");
         }
      }

      /// <summary>
      /// Gets or Sets the text on the yes button.
      /// </summary>
      public string YesButtonText
      {
         get { return this.m_TxtYesButton; }
         set
         {
            this.m_TxtYesButton = value;
            this.OnPropertyChanged ("YesButtonText");
         }
      }

      /// <summary>
      /// Gets or Sets the text on the no button.
      /// </summary>
      public string NoButtonText
      {
         get { return this.m_TxtNoButton; }
         set
         {
            this.m_TxtNoButton = value;
            this.OnPropertyChanged ("NoButtonText");
         }
      }

      /// <summary>
      /// Gets or Sets the text on the cancel button.
      /// </summary>
      public string CancelButtonText
      {
         get { return this.m_TxtCancelButton; }
         set
         {
            this.m_TxtCancelButton = value;
            this.OnPropertyChanged ("CancelButtonText");
         }
      }

      /// <summary>
      /// Gets or Sets the width of the dialog.
      /// </summary>
      public double Width
      {
         get { return this.m_Width; }
         set
         {
            this.m_Width = value;
            this.OnPropertyChanged ("Width");
         }
      }

      /// <summary>
      /// Gets or Sets the height of the dialog.
      /// </summary>
      public double Height
      {
         get { return this.m_Height; }
         set
         {
            this.m_Height = value;
            this.OnPropertyChanged ("Height");
         }
      }

      /// <summary>
      /// Gets or Sets if the ok button should be shown or not.
      /// </summary>
      public bool ShowOkButton
      {
         get { return this.m_ShowOkButton; }
         set
         {
            this.m_ShowOkButton = value;
            this.OnPropertyChanged ("ShowOkButton");
         }
      }

      /// <summary>
      /// Gets or Sets if the yes button should be shown or not.
      /// </summary>
      public bool ShowYesButton
      {
         get { return this.m_ShowYesButton; }
         set
         {
            this.m_ShowYesButton = value;
            this.OnPropertyChanged ("ShowYesButton");
         }
      }

      /// <summary>
      /// Gets or Sets if the no button should be shown or not.
      /// </summary>
      public bool ShowNoButton
      {
         get { return this.m_ShowNoButton; }
         set
         {
            this.m_ShowNoButton = value;
            this.OnPropertyChanged ("ShowNoButton");
         }
      }

      /// <summary>
      /// Gets or Sets if the cancel button should be shown or not.
      /// </summary>
      public bool ShowCancelButton
      {
         get { return this.m_ShowCancelButton; }
         set
         {
            this.m_ShowCancelButton = value;
            this.OnPropertyChanged ("ShowCancelButton");
         }
      }

      /// <summary>
      /// Gets or Sets the dialog action.
      /// </summary>
      public DialogCloseAction DialogCloseAction
      {
         get { return this.m_DialogAction; }
         set
         {
            this.m_DialogAction = value;
            this.OnPropertyChanged ("DialogAction");
         }
      }

      /// <summary>
      /// Gets or Sets the tag of the object.
      /// </summary>
      public object Tag { get; set; }

      /// <summary>
      /// Gets or Sets the modal block.
      /// </summary>
      public DispatcherFrame Block { get; set; }

      /// <summary>
      /// Checks if the ok command can execute.
      /// </summary>
      /// <returns>If it is ok to execute.</returns>
      public virtual bool CanOkExecute (object o)
      {
         return true;
      }

      /// <summary>
      /// Occurs when the ok button on the dialog is pressed.
      /// </summary>
      public virtual void OkExecuted (object o)
      {
         if (Block != null) Block.Continue = false;
         this.DialogCloseAction = DialogCloseAction.Ok;
         this.IsVisible = false;
      }

      /// <summary>
      /// Checks if the yes command can execute.
      /// </summary>
      /// <returns>If it is ok to execute.</returns>
      public virtual bool CanYesExecute (object o)
      {
         return true;
      }

      /// <summary>
      /// Occurs when the yes button on the dialog is pressed.
      /// </summary>
      public virtual void YesExecuted (object o)
      {
         if (Block != null) Block.Continue = false;
         this.DialogCloseAction = DialogCloseAction.Yes;
         this.IsVisible = false;
      }

      /// <summary>
      /// Checks if the no command can execute.
      /// </summary>
      /// <returns>If it is ok to execute.</returns>
      public virtual bool CanNoExecute (object o)
      {
         return true;
      }

      /// <summary>
      /// Occurs when the no button on the dialog is pressed.
      /// </summary>
      public virtual void NoExecuted (object o)
      {
         if (Block != null) Block.Continue = false;
         this.DialogCloseAction = DialogCloseAction.No;
         this.IsVisible = false;
      }

      /// <summary>
      /// Checks if the cancel command can execute.
      /// </summary>
      /// <returns>If it is ok to execute.</returns>
      public virtual bool CanCancelExecute (object o)
      {
         return true;
      }

      /// <summary>
      /// Occurs when the cancel button on the dialog is pressed.
      /// </summary>
      public virtual void CancelExecuted (object o)
      {
         if (Block != null) Block.Continue = false;
         this.DialogCloseAction = DialogCloseAction.Cancel;
         this.IsVisible = false;
      }

      /// <summary>
      /// Raises the closing event.
      /// </summary>
      internal void RaiseClosingEvent ()
      {
         if (this.Closing != null)
            this.Closing (this);
      }

      /// <summary>
      /// Gets the command for when the dialog ok button is pressed.
      /// </summary>
      public ICommand DialogOkCommand
      {
         get { return this.m_CmdDialogOk ?? (this.m_CmdDialogOk = new RelayCommand (this.OkExecuted, this.CanOkExecute)); }
      }

      /// <summary>
      /// Gets the command for when the dialog yes button is pressed.
      /// </summary>
      public ICommand DialogYesCommand
      {
         get { return this.m_CmdDialogYes ?? (this.m_CmdDialogYes = new RelayCommand (this.YesExecuted, this.CanYesExecute)); }
      }

      /// <summary>
      /// Gets the command for when the dialog no button is pressed.
      /// </summary>
      public ICommand DialogNoCommand
      {
         get { return this.m_CmdDialogNo ?? (this.m_CmdDialogNo = new RelayCommand (this.NoExecuted, this.CanNoExecute)); }
      }

      /// <summary>
      /// Gets the command for when the dialog ok button is pressed.
      /// </summary>
      public ICommand DialogCancelCommand
      {
         get { return this.m_CmdDialogCancel ?? (this.m_CmdDialogCancel = new RelayCommand (this.CancelExecuted, this.CanCancelExecute)); }
      }
   }
}
