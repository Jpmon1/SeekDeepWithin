using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Input;
using IinAll.Edit.Annotations;
using IinAll.Edit.Data;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// The main view model for IinAll.Edit.
   /// </summary>
   public class MainViewModel : INotifyPropertyChanged
   {
      private string m_UserName;
      private string m_SearchText;
      private RelayCommand m_CmdLogin;
      private RelayCommand m_CmdSearch;
      private RelayCommand m_CmdEditLight;
      private Light m_SelectedSearchResult;

      /// <summary>
      /// Propert changed event.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// Initializes a new main view model.
      /// </summary>
      public MainViewModel ()
      {
         this.Edit = new EditViewModel ();
         this.SearchResults = new ObservableCollection <Light> ();
      }

      /// <summary>
      /// Gets or Sets the user name.
      /// </summary>
      public string UserName
      {
         get { return this.m_UserName; }
         set
         {
            this.m_UserName = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the text to search for.
      /// </summary>
      public string SearchText
      {
         get { return this.m_SearchText; }
         set
         {
            this.m_SearchText = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the selected item in the search results.
      /// </summary>
      public Light SelectedSearchResult
      {
         get { return this.m_SelectedSearchResult; }
         set
         {
            this.m_SelectedSearchResult = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the password.
      /// </summary>
      public string Password { get; set; }

      /// <summary>
      /// Gets or Sets the edit view model.
      /// </summary>
      public EditViewModel Edit { get; private set; }

      /// <summary>
      /// Gets the list of search results.
      /// </summary>
      public ObservableCollection <Light> SearchResults { get; private set; }

      /// <summary>
      /// Gets the login command.
      /// </summary>
      public ICommand LoginCommand
      {
         get { return this.m_CmdLogin ?? (this.m_CmdLogin = new RelayCommand (this.OnLogin, this.CanLogin)); }
      }

      /// <summary>
      /// Gets the search command.
      /// </summary>
      public ICommand SearchCommand
      {
         get { return this.m_CmdSearch ?? (this.m_CmdSearch = new RelayCommand (this.OnSearch, this.CanSearch)); }
      }

      /// <summary>
      /// Gets the edit light command.
      /// </summary>
      public ICommand EditLightCommand
      {
         get { return this.m_CmdEditLight ?? (this.m_CmdEditLight = new RelayCommand (this.OnEditLight)); }
      }

      /// <summary>
      /// Adds the selected light to the edit list.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnEditLight (object obj)
      {
         this.Edit.Lights.Add (this.SelectedSearchResult);
      }

      /// <summary>
      /// Checks to see if we can search.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanSearch (object obj)
      {
         return !string.IsNullOrWhiteSpace (this.SearchText);
      }

      /// <summary>
      /// Performs the search routine.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnSearch (object obj)
      {
         this.SearchResults.Clear ();
         this.SearchResults.Add (new Light { Id = -1, Text = "Loading..." });
         var url = Constants.URL_SEARCH + HttpUtility.UrlEncode (this.SearchText);
         WebQueue.Instance.Get (url, this.OnSearchCompleted);
      }

      /// <summary>
      /// Occurs when the search method has returned.
      /// </summary>
      /// <param name="data">The data passed to the search.</param>
      /// <param name="result">The result of the search.</param>
      private void OnSearchCompleted (NameValueCollection data, dynamic result)
      {
         this.SearchResults.Clear ();
         foreach (var suggestion in result.suggestions) {
            this.SearchResults.Add(new Light { Id = suggestion.data, Text = suggestion.value});
         }
      }

      /// <summary>
      /// Checks to see if we can login.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanLogin (object obj)
      {
         return !string.IsNullOrWhiteSpace(this.UserName) &&
            !string.IsNullOrWhiteSpace (this.Password) &&
            !WebQueue.Instance.IsAuthenticated;
      }

      /// <summary>
      /// Performs the login routine.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnLogin (object obj)
      {
         byte[] passHash;
         var data = Encoding.UTF8.GetBytes (this.Password);
         using (SHA512 shaM = new SHA512Managed ())
            passHash = shaM.ComputeHash (data);
         var values = new NameValueCollection {
            {"email", this.UserName},
            {"hash", BitConverter.ToString (passHash).Replace("-", "").ToLower ()}
         };
         WebQueue.Instance.Post (Constants.URL_LOGIN_REQUEST, values);
      }

      /// <summary>
      /// Property changed method.
      /// </summary>
      /// <param name="propertyName">Name of property that changed.</param>
      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
      }
   }
}
