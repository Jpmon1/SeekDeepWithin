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

using Peter.Common.MainMenu;

namespace Peter.Common.Dialog
{
   /// <summary>
   /// Dialog for a message.
   /// </summary>
   public class ModelMessageDialog : DialogBase
   {
      private MainMenuIcon m_Icon;
      private string m_Message;
      private string m_SubMessage;

      /// <summary>
      /// Initializes a new about model.
      /// </summary>
      public ModelMessageDialog ()
      {
         this.Width = 350;
         this.Height = 200;
      }

      /// <summary>
      /// Gets or Sets the Icon for the message box.
      /// </summary>
      public MainMenuIcon Icon
      {
         get { return this.m_Icon; }
         set
         {
            this.m_Icon = value;
            this.OnPropertyChanged ("Icon");
         }
      }

      /// <summary>
      /// Gets or Sets the message.
      /// </summary>
      public string Message
      {
         get { return this.m_Message;}
         set
         {
            this.m_Message = value;
            this.OnPropertyChanged ("Message");
         }
      }

      /// <summary>
      /// Get or Sets any sub message to display.
      /// </summary>
      public string SubMessage
      {
         get { return this.m_SubMessage; }
         set
         {
            this.m_SubMessage = value;
            this.OnPropertyChanged ("SubMessage");
         }
      }
   }
}
