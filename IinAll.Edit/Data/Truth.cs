using System;
using System.Windows.Input;
using IinAll.Edit.Logic;

namespace IinAll.Edit.Data
{
   /// <summary>
   /// Represents a truth.
   /// </summary>
   public class Truth : BaseViewModel
   {
      private RelayCommand m_EditCommand;
      private RelayCommand m_LinkEditCommand;
      private int m_Order;
      private string m_Left;
      private string m_Right;
      private string m_Header;
      private string m_Footer;
      private int? m_Alias;
      private int m_FooterIndex;

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
      /// Gets or Sets the data for the left.
      /// </summary>
      public string Left
      {
         get { return this.m_Left; }
         set
         {
            if (this.m_Left != value) {
               this.m_Left = value;
               this.OnPropertyChanged ();
               this.IsModified = true;
            }
         }
      }

      /// <summary>
      /// Gets or Sets the data for the right.
      /// </summary>
      public string Right
      {
         get { return this.m_Right; }
         set
         {
            if (this.m_Right != value)
            {
               this.m_Right = value;
               this.OnPropertyChanged ();
               this.IsModified = true;
            }
         }
      }

      /// <summary>
      /// Gets or Sets the data for the header.
      /// </summary>
      public string Header
      {
         get { return this.m_Header; }
         set
         {
            if (this.m_Header != value)
            {
               this.m_Header = value;
               this.OnPropertyChanged ();
               this.IsModified = true;
            }
         }
      }

      /// <summary>
      /// Gets or Sets the data for the footer.
      /// </summary>
      public string Footer
      {
         get { return this.m_Footer; }
         set
         {
            if (this.m_Footer != value)
            {
               this.m_Footer = value;
               this.OnPropertyChanged ();
               this.IsModified = true;
            }
         }
      }

      /// <summary>
      /// Gets or Sets the foot index.
      /// </summary>
      public int FooterIndex
      {
         get { return this.m_FooterIndex; }
         set
         {
            if (this.m_FooterIndex != value)
            {
               this.m_FooterIndex = value;
               this.OnPropertyChanged ();
               if (!string.IsNullOrWhiteSpace (this.Footer))
                  this.IsModified = true;
            }
         }
      }

      /// <summary>
      /// Gets if this truth has been modified.
      /// </summary>
      public bool IsModified { get; set; }

      /// <summary>
      /// Gets the edit command.
      /// </summary>
      public ICommand EditCommand
      {
         get { return this.m_EditCommand ?? (this.m_EditCommand = new RelayCommand (this.OnEdit, this.CanEdit)); }
      }

      public ICommand LinkEditCommand
      {
         get { return this.m_LinkEditCommand ?? (this.m_LinkEditCommand = new RelayCommand (this.OnAddLink, this.CanAddLink)); }
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
