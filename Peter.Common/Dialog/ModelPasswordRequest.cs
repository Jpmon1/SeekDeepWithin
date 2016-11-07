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

namespace Peter.Common.Dialog
{
   /// <summary>
   /// Represents a model for a password request dialog.
   /// </summary>
   public sealed class ModelPasswordRequest : DialogBase
   {
      private string m_Password;
      private string m_PasswordFor;

      /// <summary>
      /// Initializes a new password request dialog.
      /// </summary>
      public ModelPasswordRequest ()
      {
         this.Width = 400;
         this.Height = 200;
         this.ShowCancelButton = true;
         this.ShowOkButton = true;
         this.ShowNoButton = false;
         this.ShowYesButton = false;
      }

      /// <summary>
      /// Gets or Sets the password.
      /// </summary>
      public string Password
      {
         get { return this.m_Password; }
         set
         {
            this.m_Password = value;
            this.OnPropertyChanged("Password");
         }
      }

      /// <summary>
      /// Gets or Sets the password for information.
      /// </summary>
      public string PasswordFor
      {
         get { return this.m_PasswordFor; }
         set
         {
            this.m_PasswordFor = value;
            this.OnPropertyChanged ("PasswordFor");
         }
      }
   }
}
