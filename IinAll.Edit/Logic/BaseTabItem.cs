using Peter.Common;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// A base class for a tab item.
   /// </summary>
   public class BaseTabItem : ViewModelBase
   {
      private string m_Title;
      private string m_ToolTip;
      private bool m_IsExpanded;

      /// <summary>
      /// Gets or Sets the title
      /// </summary>
      public string Title
      {
         get { return this.m_Title; }
         protected set
         {
            this.m_Title = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the Tool Tip
      /// </summary>
      public string ToolTip
      {
         get { return this.m_ToolTip; }
         protected set
         {
            this.m_ToolTip = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets if this item is expanded or not.
      /// </summary>
      public bool IsExpanded
      {
         get { return this.m_IsExpanded; }
         set
         {
            this.m_IsExpanded = value;
            this.OnPropertyChanged ();
         }
      }
   }
}
