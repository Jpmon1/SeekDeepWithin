namespace IinAll.Edit.Logic
{
   /// <summary>
   /// A base class for a tab item.
   /// </summary>
   public class BaseTabItem : BaseViewModel
   {
      private string m_Title;
      private bool m_IsSelected;

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
      /// Gets or Sets if this item is selected or not.
      /// </summary>
      public bool IsSelected
      {
         get { return this.m_IsSelected; }
         set
         {
            this.m_IsSelected = value;
            this.OnPropertyChanged ();
         }
      }
   }
}
