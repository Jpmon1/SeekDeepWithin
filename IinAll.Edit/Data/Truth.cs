using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using IinAll.Edit.Interfaces;
using IinAll.Edit.Logic;
using Peter.Common;

namespace IinAll.Edit.Data
{
   /// <summary>
   /// Represents a truth.
   /// </summary>
   public class Truth : ViewModelBase, IBodyContainer
   {
      private int m_Order;
      private int? m_Alias;
      private bool m_IsSelected;
      private RelayCommand m_EditCommand;
      private RelayCommand m_LinkCommand;
      private RelayCommand m_RemoveCommand;
      private RelayCommand m_AddBodyCommand;

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
      /// Gets or Sets the command used for links.
      /// </summary>
      public ICommand LinkCommand
      {
         get { return this.m_LinkCommand ?? (this.m_LinkCommand = new RelayCommand (this.OnLink, this.CanLink)); }
      }

      /// <summary>
      /// Gets or Sets the command used to add a body.
      /// </summary>
      public ICommand AddBodyCommand
      {
         get { return this.m_AddBodyCommand ?? (this.m_AddBodyCommand = new RelayCommand (this.OnAddBody, this.CanAddBody)); }
      }

      /// <summary>
      /// Gets or Sets the command used to remove the truth.
      /// </summary>
      public ICommand RemoveCommand
      {
         get { return this.m_RemoveCommand ?? (this.m_RemoveCommand = new RelayCommand (this.OnRemove, this.CanRemove)); }
      }

      /// <summary>
      /// Verifies if the remove command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanRemove (object obj)
      {
         return this.Id <= 0 || WebQueue.Instance.IsAuthenticated;
      }

      /// <summary>
      /// Executes the remove command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnRemove (object obj)
      {
         if (this.Id > 0) {
            var result = MessageBox.Show (Application.Current.MainWindow,
               "Are you certain you want to remove this truth?",
               "I in All", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes) {
               var parameters = new NameValueCollection {{"id", this.Id.ToString ()}};
               WebQueue.Instance.Delete (Constants.URL_TRUTH, parameters, this.OnDelete);
            }
         } else {
            this.Parent.RemoveTruth (this);
         }
      }

      /// <summary>
      /// Occurs when the remove truth request returns successfully.
      /// </summary>
      /// <param name="parameters">The original parameters.</param>
      /// <param name="result">The result.</param>
      private void OnDelete (NameValueCollection parameters, dynamic result)
      {
         this.Parent.RemoveTruth (this);
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
         this.Bodies.Add (new Body (this, this.Love.Id));
      }

      /// <summary>
      /// Verifies if the add link command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanLink (object obj)
      {
         return WebQueue.Instance.IsAuthenticated &&
                (this.Alias != null ||
                 (this.Love != null && this.Love.Id > 0 && this.Parent.EditLight.Count > 0 &&
                  this.Parent.EditLoveId != -1));
      }

      /// <summary>
      /// Executes the add link command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnLink (object obj)
      {
         var parameters = new NameValueCollection {
            {"l", this.Love.Id.ToString ()},
            {"a", this.Parent.EditLoveId.ToString ()}
         };
         if (this.Alias.HasValue)
            WebQueue.Instance.Delete (Constants.URL_ALIAS, parameters, this.OnLinkDelete);
         else
            WebQueue.Instance.Post (Constants.URL_ALIAS, parameters, this.OnLinkAdd);
      }

      /// <summary>
      /// Occurs when the add alias was successful.
      /// </summary>
      /// <param name="parameters">The original parameters.</param>
      /// <param name="results">The result.</param>
      private void OnLinkDelete (NameValueCollection parameters, dynamic results)
      {
         this.Alias = null;
      }

      /// <summary>
      /// Occurs when the add alias was successful.
      /// </summary>
      /// <param name="parameters">The original parameters.</param>
      /// <param name="results">The result.</param>
      private void OnLinkAdd (NameValueCollection parameters, dynamic results)
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
      public void RemoveBody (Body body)
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
