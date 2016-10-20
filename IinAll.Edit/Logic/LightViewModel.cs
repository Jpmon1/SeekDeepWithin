using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Windows.Input;
using IinAll.Edit.Annotations;
using IinAll.Edit.Data;

namespace IinAll.Edit.Logic
{
   public class LightViewModel : ILightContainer
   {
      private string m_SearchText;
      private Light m_SelectedSearchResult;
      private RelayCommand m_SearchCommand;
      private RelayCommand m_CreateCommand;

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
         this.Light.CollectionChanged += this.OnLightChanged;
         this.SearchResults = new ObservableCollection <Light> ();
      }

      /// <summary>
      /// Occurs when the light list changes.
      /// </summary>
      /// <param name="sender">The light list.</param>
      /// <param name="e">NotifyCollectionChangedEventArgs</param>
      private void OnLightChanged (object sender, NotifyCollectionChangedEventArgs e)
      {
         if (e.NewItems != null) {
            foreach (var light in e.NewItems.Cast <Light> ()) {
               light.Parent = this;
            }
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
      /// Gets the list of staged light.
      /// </summary>
      public ObservableCollection<Light> Light { get; }

      /// <summary>
      /// Gets the list of search results.
      /// </summary>
      public ObservableCollection<Light> SearchResults { get; }

      /// <summary>
      /// Gets the search command.
      /// </summary>
      public ICommand SearchCommand
      {
         get { return this.m_SearchCommand ?? (this.m_SearchCommand = new RelayCommand (this.OnSearch, this.CanSearch)); }
      }

      /// <summary>
      /// Gets the create command.
      /// </summary>
      public ICommand CreateCommand
      {
         get { return this.m_CreateCommand ?? (this.m_CreateCommand = new RelayCommand (this.OnCreate, this.CanCreate)); }
      }

      /// <summary>
      /// Verifies if the create light command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter (not used).</param>
      /// <returns>True if can execute, otherwise false.</returns>
      private bool CanCreate (object obj)
      {
         return !string.IsNullOrWhiteSpace (this.SearchText) && WebQueue.Instance.IsAuthenticated;
      }

      /// <summary>
      /// Create a new light on the server.
      /// </summary>
      /// <param name="obj"></param>
      private void OnCreate (object obj)
      {
         var data = new NameValueCollection {
            { "l", this.SearchText },
            { "user", WebQueue.Instance.UserId.ToString () },
            { "token", WebQueue.Instance.Token}
         };
         WebQueue.Instance.Post (Constants.URL_LIGHT, data, this.OnLightCreated);
      }

      /// <summary>
      /// Callback for when a light is created.
      /// </summary>
      /// <param name="parameters">The parameters passed.</param>
      /// <param name="result">The result of the creation.</param>
      private void OnLightCreated (NameValueCollection parameters, dynamic result)
      {
         this.Light.Add (new Light {
            Id = result.id,
            Text = parameters ["l"]
         });
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
      /// Removes the given light.
      /// </summary>
      /// <param name="light">Light to remove.</param>
      /// <param name="type"></param>
      public void RemoveLight (Light light, LightType type)
      {
         this.Light.Remove (light);
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
