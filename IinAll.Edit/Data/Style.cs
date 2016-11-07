using System.Collections.Specialized;
using System.Windows.Input;
using IinAll.Edit.Interfaces;
using IinAll.Edit.Logic;
using Peter.Common;

namespace IinAll.Edit.Data
{
   public class Style : ViewModelBase
   {
      private int m_Id;
      private string m_Css;
      private int m_EndIndex;
      private string m_Tag;
      private int m_StartIndex;
      private readonly int m_LoveId;
      private readonly IStyleContainer m_Parent;
      private RelayCommand m_SaveCommand;
      private RelayCommand m_DeleteCommand;

      /// <summary>
      /// Initializes a new style for the given love.
      /// </summary>
      /// <param name="parent">The parent.</param>
      /// <param name="loveId">The id of the love the style is for.</param>
      public Style (IStyleContainer parent, int loveId = -1)
      {
         this.m_Parent = parent;
         this.m_LoveId = loveId;
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
      /// Gets or Sets the start of the style.
      /// </summary>
      public string Tag
      {
         get { return this.m_Tag; }
         set
         {
            if (this.m_Tag != value) {
               this.m_Tag = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Gets or Sets the start index of the style.
      /// </summary>
      public int StartIndex
      {
         get { return this.m_StartIndex; }
         set
         {
            if (this.m_StartIndex != value) {
               this.m_StartIndex = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Gets or Sets the end of the style.
      /// </summary>
      public string Css
      {
         get { return this.m_Css; }
         set
         {
            if (this.m_Css != value) {
               this.m_Css = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Gets or Sets the end index of the style.
      /// </summary>
      public int EndIndex
      {
         get { return this.m_EndIndex; }
         set
         {
            if (this.m_EndIndex != value) {
               this.m_EndIndex = value;
               this.OnPropertyChanged ();
            }
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
      /// Verifies if the save command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanSave (object obj)
      {
         return this.m_LoveId != -1 && this.Id <= 0 &&
            !string.IsNullOrWhiteSpace (this.Tag) &&
            WebQueue.Instance.IsAuthenticated;
      }

      /// <summary>
      /// Executes the save command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnSave (object obj)
      {
         var parameters = new NameValueCollection {
               {"c", this.Css},
               {"t", this.Tag},
               {"s", this.StartIndex.ToString ()},
               {"e", this.EndIndex.ToString ()},
               {"l", this.m_LoveId.ToString ()}
            };
         WebQueue.Instance.Post (Constants.URL_STYLE, parameters, this.OnSaveSuccess);
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
            this.m_Parent.RemoveStyle (this);
         } else {
            var parameters = new NameValueCollection { { "id", this.Id.ToString () } };
            WebQueue.Instance.Delete (Constants.URL_STYLE, parameters, this.OnDeleteSuccess);
         }
      }

      /// <summary>
      /// Occurs when the delete was successful.
      /// </summary>
      /// <param name="parameters">The original parameters.</param>
      /// <param name="results">The result.</param>
      private void OnDeleteSuccess (NameValueCollection parameters, dynamic results)
      {
         this.m_Parent.RemoveStyle (this);
      }
   }
}
