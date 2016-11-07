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

using System;

namespace Peter.Common.Dialog
{
   /// <summary>
   /// Model used to request input from the user.
   /// </summary>
   public class ModelTextInputRequest : DialogBase
   {
      private string m_Input;
      private string m_Prompt;
      private string m_Placeholder;

      /// <summary>
      /// Initializes a new password request dialog.
      /// </summary>
      public ModelTextInputRequest ()
      {
         this.Width = 400;
         this.Height = 200;
         this.ShowCancelButton = true;
         this.ShowOkButton = true;
         this.ShowNoButton = false;
         this.ShowYesButton = false;
         this.m_Prompt = "Please enter the requested text";
      }

      /// <summary>
      /// Gets or Sets the prompt.
      /// </summary>
      public string Prompt
      {
         get { return this.m_Prompt; }
         set
         {
            this.m_Prompt = value;
            this.OnPropertyChanged ("Prompt");
         }
      }

      /// <summary>
      /// Gets or Sets the text the user input.
      /// </summary>
      public string Input
      {
         get { return this.m_Input; }
         set
         {
            this.m_Input = value;
            this.OnPropertyChanged ("Input");
         }
      }

      /// <summary>
      /// Gets text for a place holder.
      /// </summary>
      public string Placeholder
      {
         get { return this.m_Placeholder; }
         set
         {
            this.m_Placeholder = value;
            this.OnPropertyChanged ("Placeholder");
         }
      }

      /// <summary>
      /// Gets or Sets a function that checks if ok can be pressed or not.
      /// </summary>
      public Func <string, bool> CanOkCheck { get; set; }

      /// <summary>
      /// Checks if the ok command can execute.
      /// </summary>
      /// <returns>If it is ok to execute.</returns>
      public override bool CanOkExecute (object o)
      {
         return CanOkCheck == null || this.CanOkCheck (this.Input);
      }
   }
}
