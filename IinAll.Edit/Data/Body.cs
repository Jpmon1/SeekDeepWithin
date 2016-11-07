using System.Collections.Specialized;
using System.Windows.Input;
using IinAll.Edit.Interfaces;
using IinAll.Edit.Logic;
using Peter.Common;

namespace IinAll.Edit.Data
{
   /// <summary>
   /// A body item (header, footer, left, right)
   /// </summary>
   public class Body : ViewModelBase
   {
      private int m_Id;
      private int m_Index;
      private string m_Text;
      private BodyType m_BodyType;
      private readonly int m_LoveId;
      private readonly IBodyContainer m_Parent;
      private RelayCommand m_SaveCommand;
      private RelayCommand m_DeleteCommand;
      private bool m_NeedsIndex;

      /// <summary>
      /// Initializes a new body for the given love.
      /// </summary>
      /// <param name="parent">The parent truth.</param>
      /// <param name="loveId">The id of the love the body is for.</param>
      public Body (IBodyContainer parent, int loveId = -1)
      {
         this.m_Parent = parent;
         this.m_LoveId = loveId;
         this.BodyType = BodyType.Left;
      }

      /// <summary>
      /// Gets or Sets the Id.
      /// </summary>
      public int Id
      {
         get { return this.m_Id; }
         set
         {
            this.m_Id = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the type.
      /// </summary>
      public BodyType BodyType
      {
         get { return this.m_BodyType; }
         set
         {
            if (this.m_BodyType != value) {
               this.m_BodyType = value;
               this.OnPropertyChanged (nameof (BodyTypeInt));
               this.NeedsIndex = this.m_BodyType == BodyType.Footer;
            }
         }
      }

      /// <summary>
      /// Gets or Sets the int value of the type.
      /// </summary>
      public int BodyTypeInt
      {
         get { return (int)this.BodyType; }
         set { this.BodyType = (BodyType) value; }
      }

      /// <summary>
      /// Gets or Sets the text.
      /// </summary>
      public string Text
      {
         get { return this.m_Text; }
         set
         {
            this.m_Text = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the index if it is a footer.
      /// </summary>
      public int Index
      {
         get { return this.m_Index; }
         set
         {
            this.m_Index = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the save command.
      /// </summary>
      public ICommand SaveCommand
      {
         get { return this.m_SaveCommand ?? (this.m_SaveCommand = new RelayCommand (this.OnSave, this.CanSave)); }
      }

      /// <summary>
      /// Gets or Sets the delete command.
      /// </summary>
      public ICommand DeleteCommand
      {
         get { return this.m_DeleteCommand ?? (this.m_DeleteCommand = new RelayCommand (this.OnDelete, this.CanDelete)); }
      }

      /// <summary>
      /// Gets or Sets the position
      /// </summary>
      public int Position
      {
         get { return this.GetPosition (); }
         set { this.SetPosition (value); }
      }

      /// <summary>
      /// Gets or Sets if we need to use the index.
      /// </summary>
      public bool NeedsIndex
      {
         get { return this.m_NeedsIndex; }
         set
         {
            this.m_NeedsIndex = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Verifies if the save command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanSave (object obj)
      {
         return this.m_LoveId != -1 && this.Id <= 0 &&
            !string.IsNullOrWhiteSpace (this.Text) &&
            WebQueue.Instance.IsAuthenticated;
      }

      /// <summary>
      /// Executes the save command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnSave (object obj)
      {
         var parameters = new NameValueCollection {
               {"t", this.Text},
               {"l", this.m_LoveId.ToString ()},
               {"p", this.GetPosition().ToString ()}
            };
         WebQueue.Instance.Post (Constants.URL_BODY, parameters, this.OnSaveSuccess);
      }

      /// <summary>
      /// Occurs when the save was successful.
      /// </summary>
      /// <param name="parameters">The original parameters.</param>
      /// <param name="results">The result.</param>
      private void OnSaveSuccess (NameValueCollection parameters, dynamic results)
      {
         this.Id = results.id;
         CommandManager.InvalidateRequerySuggested ();
      }

      /// <summary>
      /// Verifies if the delete command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanDelete (object obj)
      {
         return this.Id <= 0 || WebQueue.Instance.IsAuthenticated;
      }

      /// <summary>
      /// Executes the delete command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnDelete (object obj)
      {
         if (this.Id <= 0) {
            this.m_Parent.RemoveBody (this);
         } else {
            var parameters = new NameValueCollection { {"id", this.Id.ToString ()} };
            WebQueue.Instance.Delete (Constants.URL_BODY, parameters, this.OnDeleteSuccess);
         }
      }

      /// <summary>
      /// Occurs when the delete was successful.
      /// </summary>
      /// <param name="parameters">The original parameters.</param>
      /// <param name="results">The result.</param>
      private void OnDeleteSuccess (NameValueCollection parameters, dynamic results)
      {
         this.m_Parent.RemoveBody (this);
      }

      /// <summary>
      /// Sets the body's position.
      /// </summary>
      /// <param name="position">The position to set.</param>
      private void SetPosition (int position)
      {
         if (position == -1)
            this.BodyType = BodyType.Left;
         else if (position == -2)
            this.BodyType = BodyType.Header;
         else if (position == -3)
            this.BodyType = BodyType.Right;
         else if (position == -4)
            this.BodyType = BodyType.Bottom;
         else {
            this.BodyType = BodyType.Footer;
            this.Index = position;
         }
      }

      /// <summary>
      /// Gets the  body's position.
      /// </summary>
      /// <returns>The position of the body.</returns>
      public int GetPosition ()
      {
         if (this.BodyType == BodyType.Left)
            return -1;
         if (this.BodyType == BodyType.Header)
            return -2;
         if (this.BodyType == BodyType.Right)
            return -3;
         if (this.BodyType == BodyType.Bottom)
            return -4;
         return this.Index;
      }
   }
}
