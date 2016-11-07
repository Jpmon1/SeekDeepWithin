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

using Peter.Common.Dialog;

namespace Peter.Common
{
   /// <summary>
   /// Base for a window view model.
   /// </summary>
   public class MainWindowViewModelBase : ViewModelBase
   {
      private DialogBase m_DialogContent;

      /// <summary>
      /// Gets or Sets the Dialog content.
      /// </summary>
      public DialogBase DialogContent
      {
         get { return this.m_DialogContent; }
         set
         {
            if (value == null && this.m_DialogContent != null)
               this.DialogContent.RaiseClosingEvent ();
            this.m_DialogContent = value;
            if (this.m_DialogContent != null)
               this.m_DialogContent.IsVisible = true;
            this.OnPropertyChanged ("DialogContent");
         }
      }
   }
}
