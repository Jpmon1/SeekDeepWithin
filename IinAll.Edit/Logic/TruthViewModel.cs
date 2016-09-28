using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Input;
using IinAll.Edit.Data;
using IinAll.Edit.Properties;
using Newtonsoft.Json.Linq;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// View model for truth.
   /// </summary>
   public class TruthViewModel : BaseTabItem, ILightContainer
   {
      private string m_TextToFormat;
      private string m_JsonText;
      private RelayCommand m_CmdSave;
      private RelayCommand m_CmdFormat;

      /// <summary>
      /// Initializes a new truth view model.
      /// </summary>
      public TruthViewModel ()
      {
         this.Title = "Truth";
         this.Light = new ObservableCollection <Light> ();
         this.Truth = new ObservableCollection <Truth> ();
         this.Light.CollectionChanged += this.OnLightChanged;
         this.Regexes = new ObservableCollection<string> ();
         if (Settings.Default.RegExes != null) {
            foreach (var regEx in Settings.Default.RegExes)
               this.Regexes.Add (regEx);
         }
      }

      /// <summary>
      /// Gets the list of staged light.
      /// </summary>
      public ObservableCollection<Light> Light { get; }

      /// <summary>
      /// Gets the collection of truth.
      /// </summary>
      public ObservableCollection<Truth> Truth { get; private set; }

      /// <summary>
      /// Gets the current collection of RegExes.
      /// </summary>
      public ObservableCollection<string> Regexes { get; }

      /// <summary>
      /// Gets or Sets the text to format.
      /// </summary>
      public string TextToFormat
      {
         get { return this.m_TextToFormat; }
         set
         {
            this.m_TextToFormat = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the current regular expression.
      /// </summary>
      public string CurrentRegex
      {
         get { return Settings.Default.CurrentRegEx; }
         set
         {
            Settings.Default.CurrentRegEx = value;
            OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the formatted text.
      /// </summary>
      public string JsonText
      {
         get { return this.m_JsonText; }
         set
         {
            this.m_JsonText = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets the format command.
      /// </summary>
      public ICommand FormatCommand
      {
         get { return this.m_CmdFormat ?? (this.m_CmdFormat = new RelayCommand (this.OnFormat, this.CanFormat)); }
      }

      /// <summary>
      /// Gets the save command.
      /// </summary>
      public ICommand SaveCommand
      {
         get { return this.m_CmdSave ?? (this.m_CmdSave = new RelayCommand (this.OnSave, this.CanSave)); }
      }

      /// <summary>
      /// Verifies if the save command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanSave (object obj)
      {
         return this.Light.Count > 0 && WebQueue.Instance.IsAuthenticated &&
            !string.IsNullOrWhiteSpace (this.JsonText);
      }

      /// <summary>
      /// Executes the save command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnSave (object obj)
      {
         if (!WebQueue.Instance.IsAuthenticated) {
            MessageBox.Show (Application.Current.MainWindow, "You are not logged in!", "I in All", MessageBoxButton.OK,
               MessageBoxImage.Error);
            return;
         }
         var inString = false;
         var formatted = string.Empty;
         foreach (var letter in this.JsonText)
         {
            if (letter == '\r' || letter == '\n') continue;
            if (letter == ' ' && !inString) continue;
            if (letter == '"') inString = !inString;
            formatted += letter;
         }
         formatted = "[" + formatted + "]";
         var data = new NameValueCollection {
            { "d", formatted },
            { "user", WebQueue.Instance.UserId.ToString () },
            { "token", WebQueue.Instance.Token}
         };
         WebQueue.Instance.Post (Constants.URL_TRUTH, data, this.OnSaveSuccess);
      }

      /// <summary>
      /// Occurs when the search method has returned.
      /// </summary>
      /// <param name="data">The data passed to the search.</param>
      /// <param name="result">The result of the search.</param>
      private void OnSaveSuccess (NameValueCollection data, object result)
      {
         this.GetTruth ();
      }

      /// <summary>
      /// Verifies if the format command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanFormat (object obj)
      {
         return !string.IsNullOrWhiteSpace (this.CurrentRegex) &&
            !string.IsNullOrWhiteSpace (this.TextToFormat);
      }

      /// <summary>
      /// Executes the format command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnFormat (object obj)
      {
         if (!this.Regexes.Contains (this.CurrentRegex))
         {
            this.Regexes.Add (this.CurrentRegex);
            Settings.Default.RegExes.Add (this.CurrentRegex);
         }
         this.Format ();
      }

      /// <summary>
      /// Occurs when the light list changes.
      /// </summary>
      /// <param name="sender">The light list.</param>
      /// <param name="e">NotifyCollectionChangedEventArgs</param>
      private void OnLightChanged (object sender, NotifyCollectionChangedEventArgs e)
      {
         if (e.NewItems != null) {
            foreach (var light in e.NewItems.Cast<Light> ()) {
               light.Parent = this;
            }
         }
         this.GetTruth ();
      }

      /// <summary>
      /// Gets the truths for the current light.
      /// </summary>
      private void GetTruth ()
      {
         this.Truth.Clear ();
         var love = GetLove ();
         if (!string.IsNullOrWhiteSpace (love)) {
            var url = Constants.URL_GET_TRUTH + HttpUtility.UrlEncode (love);
            WebQueue.Instance.Get (url, this.OnGetTruths);
         }
      }

      /// <summary>
      /// Gets the truths for the current love.
      /// </summary>
      /// <param name="parameters">parameters</param>
      /// <param name="result">results</param>
      private void OnGetTruths (NameValueCollection parameters, dynamic result)
      {
         foreach (var truth in result.truths) {
            this.Truth.Add (new Truth {
               Id = truth.id,
               Light = new Light {
                  Id = truth.lid,
                  Text = truth.t
               },
               Love = new Love {
                  Id = result.love
               }
            });
         }
      }

      /// <summary>
      /// Removes the given light.
      /// </summary>
      /// <param name="light">Light to remove.</param>
      public void RemoveLight (Light light)
      {
         this.Light.Remove (light);
      }

      /// <summary>
      /// Gets the current love.
      /// </summary>
      /// <returns>The current love.</returns>
      private string GetLove ()
      {
         var love = string.Empty;
         foreach (var light in this.Light){
            if (!string.IsNullOrWhiteSpace (love))
               love += ",";
            love += light.Id;
         }
         return love;
      }

      /// <summary>
      /// Formats the text to format with the selected regular expression.
      /// </summary>
      private void Format ()
      {
         var matches = Regex.Matches (this.TextToFormat, this.CurrentRegex, RegexOptions.IgnoreCase);
         var itemList = new JArray ();
         foreach (Match match in matches) {
            var item = new JObject {
               ["t"] = match.Groups["t"].Value.Replace ("\"", "&quot;").Trim ()
            };
            itemList.Add (item);
         }
         var blob = new JObject { ["love"] = GetLove (), ["items"] = itemList };
         this.JsonText = blob.ToString ();
      }
   }
}
