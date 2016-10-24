using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IinAll.Edit.Logic;

namespace IinAll.Edit.Data
{
   /// <summary>
   /// Represents a truth.
   /// </summary>
   public class Truth : BaseViewModel
   {
      private int m_Order;
      private int? m_Alias;
      private RelayCommand m_EditCommand;
      private RelayCommand m_AddBodyCommand;
      private RelayCommand m_LinkEditCommand;

      /// <summary>
      /// Initializes a new truth model.
      /// </summary>
      /// <param name="parent">The parent view model.</param>
      /// <param name="id">The id of the trut.</param>
      public Truth (LoveViewModel parent, int id = 0)
      {
         if (parent == null)
            throw new ArgumentNullException (nameof (parent), @"Parent of a truth cannot be null.");
         this.Id = id;
         this.Parent = parent;
         this.Bodies = new ObservableCollection <Body> ();
      }

      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; }

      /// <summary>
      /// Gets the parent view model.
      /// </summary>
      public LoveViewModel Parent { get; }

      /// <summary>
      /// Gets or Sets if this is a new truth. If false, truth is probably from the server.
      /// </summary>
      public bool IsNew { get; set; }

      /// <summary>
      /// Gets or Sets the child love.
      /// </summary>
      public Love Love { get; set; }

      /// <summary>
      /// Gets or Sets the Id of the alias.
      /// </summary>
      public int? Alias
      {
         get { return this.m_Alias; }
         set
         {
            if (this.m_Alias != value) {
               this.m_Alias = value;
               this.OnPropertyChanged ();
               this.IsModified = true;
            }
         }
      }

      /// <summary>
      /// Gets or Sets the order.
      /// </summary>
      public int Order
      {
         get { return this.m_Order; }
         set
         {
            if (this.m_Order != value) {
               this.m_Order = value;
               this.OnPropertyChanged ();
               this.IsModified = true;
            }
         }
      }

      /// <summary>
      /// Gets if this truth has been modified.
      /// </summary>
      public bool IsModified { get; set; }

      /// <summary>
      /// Gets the list of bodies.
      /// </summary>
      public ObservableCollection <Body> Bodies { get; }

      /// <summary>
      /// Gets the edit command.
      /// </summary>
      public ICommand EditCommand
      {
         get { return this.m_EditCommand ?? (this.m_EditCommand = new RelayCommand (this.OnEdit, this.CanEdit)); }
      }

      /// <summary>
      /// Gets or Sets the command used to grab the alias.
      /// </summary>
      public ICommand LinkEditCommand
      {
         get { return this.m_LinkEditCommand ?? (this.m_LinkEditCommand = new RelayCommand (this.OnAddLink, this.CanAddLink)); }
      }

      /// <summary>
      /// Gets or Sets the command used to add a body.
      /// </summary>
      public ICommand AddBodyCommand
      {
         get { return this.m_AddBodyCommand ?? (this.m_AddBodyCommand = new RelayCommand (this.OnAddBody, this.CanAddBody)); }
      }

      /// <summary>
      /// Verifies if the add body command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanAddBody (object obj)
      {
         return this.Id != -1;
      }

      /// <summary>
      /// Executes the add body command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnAddBody (object obj)
      {
         this.Bodies.Add(new Body (this, this.Love.Id));
      }

      /// <summary>
      /// Verifies if the add link command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanAddLink (object obj)
      {
         return this.Parent.EditLight.Count > 0 && this.Parent.EditLoveId != -1;
      }

      /// <summary>
      /// Executes the add link command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnAddLink (object obj)
      {
         this.Alias = this.Parent.EditLoveId;
      }

      /// <summary>
      /// Verifies to see if the edit command can execute.
      /// </summary>
      /// <param name="obj">Command parameter, not used.</param>
      /// <returns>True if command can execute, otherwise false.</returns>
      private bool CanEdit (object obj)
      {
         return !this.IsNew;
      }

      /// <summary>
      /// Executes the edit action.
      /// </summary>
      /// <param name="obj">Command parameter, not used.</param>
      private void OnEdit (object obj)
      {
         MainViewModel.Instance.EditTruth (this);
      }

      /// <summary>
      /// Removes a body from the bodies.
      /// </summary>
      /// <param name="body">The body to remove.</param>
      internal void RemoveBody (Body body)
      {
         this.Bodies.Remove (body);
      }

      /// <summary>
      /// Gets the love for this truth.
      /// </summary>
      /// <returns></returns>
      public string GetLove ()
      {
         var love = string.Empty;
         foreach (var light in this.Love.Peace)
            love += light.Text + "|";
         love += this.Love.Light.Text;
         return love;
      }
   }
}
