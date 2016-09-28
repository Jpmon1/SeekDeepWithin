using System.Windows.Input;
using IinAll.Edit.Logic;

namespace IinAll.Edit.Data
{
   /// <summary>
   /// Represents a light.
   /// </summary>
   public class Light
   {
      private RelayCommand m_CmdRemove;

      /// <summary>
      /// Gets or Sets the id of the light.
      /// </summary>
      public int Id { get; set; }
      
      /// <summary>
      /// Gets or Sets the text of the light.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the parent edit model.
      /// </summary>
      public ILightContainer Parent { get; set; }

      /// <summary>
      /// Gets the remove light command.
      /// </summary>
      public ICommand RemoveCommand
      {
         get { return this.m_CmdRemove ?? (this.m_CmdRemove = new RelayCommand (this.OnRemove, this.CanRemove)); }
      }

      /// <summary>
      /// Verifies that the remove command can be executed.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private bool CanRemove (object obj)
      {
         return this.Parent != null;
      }

      /// <summary>
      /// Executes the remove command.
      /// </summary>
      /// <param name="obj">Command parameter, not used.</param>
      private void OnRemove (object obj)
      {
         this.Parent.RemoveLight (this);
      }
   }
}
