using System.Collections.Specialized;
using System.Windows.Input;
using IinAll.Edit.Interfaces;
using IinAll.Edit.Logic;
using Peter.Common;

namespace IinAll.Edit.Data
{
   /// <summary>
   /// Represents a light.
   /// </summary>
   public class Light
   {
      private bool m_IsBusy;
      private RelayCommand m_CmdSave;
      private RelayCommand m_CmdRemove;

      /// <summary>
      /// Initializes a new light. Default constructor.
      /// </summary>
      public Light () { }

      /// <summary>
      /// Initializes a new light.
      /// </summary>
      /// <param name="light">The light to copy data from.</param>
      public Light (Light light)
      {
         this.Id = light.Id;
         this.Text = light.Text;
      }

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
      /// Gets or Sets the light type.
      /// </summary>
      public LightType Type { get; set; }

      /// <summary>
      /// Gets the remove light command.
      /// </summary>
      public ICommand RemoveCommand
      {
         get { return this.m_CmdRemove ?? (this.m_CmdRemove = new RelayCommand (this.OnRemove, this.CanRemove)); }
      }

      /// <summary>
      /// Gets the save light command.
      /// </summary>
      public ICommand SaveCommand
      {
         get { return this.m_CmdSave ?? (this.m_CmdSave = new RelayCommand (this.OnSave, this.CanSave)); }
      }

      /// <summary>
      /// Verifies if the light can be saved.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if command can execute, otherwise false.</returns>
      private bool CanSave (object obj)
      {
         return WebQueue.Instance.IsAuthenticated && !this.m_IsBusy;
      }

      /// <summary>
      /// Saves the light to the server.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnSave (object obj)
      {
         this.m_IsBusy = true;
         var parameters = new NameValueCollection {
               {"text", this.Text},
               {"id", this.Id.ToString ()}
            };
         WebQueue.Instance.Put (Constants.URL_LIGHT, parameters, this.OnSaveSuccess);
      }

      /// <summary>
      /// Occurs when the save was successful.
      /// </summary>
      /// <param name="arg1"></param>
      /// <param name="result"></param>
      private void OnSaveSuccess (NameValueCollection arg1, dynamic result)
      {
         this.m_IsBusy = false;
         CommandManager.InvalidateRequerySuggested ();
      }

      /// <summary>
      /// Verifies that the remove command can be executed.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if command can execute, otherwise false.</returns>
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
         this.Parent.RemoveLight (this, this.Type);
      }
   }
}
