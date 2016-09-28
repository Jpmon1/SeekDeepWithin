using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Web;
using System.Windows.Input;
using IinAll.Edit.Annotations;
using IinAll.Edit.Data;

namespace IinAll.Edit.Logic
{
   public class LightViewModel
   {
      private string m_SearchText;
      private Light m_SelectedSearchResult;
      private RelayCommand m_SearchCommand;
      private RelayCommand m_StageLightCommand;

      /// <summary>
      /// Propert changed event.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// Initializes a new light view model.
      /// </summary>
      public LightViewModel ()
      {
         this.Light = new ObservableCollection <Light> ();
         this.SearchResults = new ObservableCollection <Light> ();
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
      /// Gets the list of staged light.
      /// </summary>
      public ObservableCollection<Light> Light { get; private set; }

      /// <summary>
      /// Gets the list of search results.
      /// </summary>
      public ObservableCollection<Light> SearchResults { get; private set; }

      /// <summary>
      /// Gets the search command.
      /// </summary>
      public ICommand SearchCommand
      {
         get { return this.m_SearchCommand ?? (this.m_SearchCommand = new RelayCommand (this.OnSearch, this.CanSearch)); }
      }

      /// <summary>
      /// Gets the stage light command.
      /// </summary>
      public ICommand StageLightCommand
      {
         get { return this.m_StageLightCommand ?? (this.m_StageLightCommand = new RelayCommand (this.OnStage, this.CanStage)); }
      }

      /// <summary>
      /// Checks to see if we can stage.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanStage (object obj)
      {
         return this.SelectedSearchResult != null;
      }

      /// <summary>
      /// Performs the stage routine.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnStage (object obj)
      {
         this.Light.Add (this.SelectedSearchResult);
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
         foreach (var suggestion in result.suggestions)
         {
            this.SearchResults.Add (new Light { Id = suggestion.data, Text = suggestion.value });
         }
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
