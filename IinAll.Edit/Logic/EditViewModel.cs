using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Input;
using IinAll.Edit.Annotations;
using IinAll.Edit.Data;
using IinAll.Edit.Properties;
using Newtonsoft.Json.Linq;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// View model for editing.
   /// </summary>
   public class EditViewModel : INotifyPropertyChanged
   {
      private string m_TextToFormat;
      private string m_StartOrder;
      private string m_StartNumber;
      private string m_JsonText;
      private RelayCommand m_CmdSave;
      private RelayCommand m_CmdFormat;

      /// <summary>
      /// Propert changed event.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// Initializes a new edit view model.
      /// </summary>
      public EditViewModel ()
      {
         this.m_StartOrder = "1";
         this.Truths = new ObservableCollection <Truth> ();
         this.Lights = new ObservableCollection <Light> ();
         this.Lights.CollectionChanged += this.OnLightsChanged;
         this.Regexes = new ObservableCollection <string> ();
         if (Settings.Default.RegExes != null) {
            foreach (var regEx in Settings.Default.RegExes)
               this.Regexes.Add (regEx);
         }
      }

      /// <summary>
      /// Gets the list of lights being edited.
      /// </summary>
      public ObservableCollection <Light> Lights { get; private set; }

      /// <summary>
      /// Gets the collection of truths.
      /// </summary>
      public ObservableCollection <Truth> Truths { get; private set; }

      /// <summary>
      /// Gets the current collection of RegExes.
      /// </summary>
      public ObservableCollection <string> Regexes { get; private set; }

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
      /// Gets or Sets the starting order for the format command.
      /// </summary>
      public string StartOrder
      {
         get { return this.m_StartOrder; }
         set
         {
            this.m_StartOrder = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the starting number for the format command.
      /// </summary>
      public string StartNumber
      {
         get { return this.m_StartNumber; }
         set
         {
            this.m_StartNumber = value;
            this.OnPropertyChanged ();
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
         return this.Lights.Count > 0 && WebQueue.Instance.IsAuthenticated &&
            !string.IsNullOrWhiteSpace (this.JsonText);
      }

      /// <summary>
      /// Executes the save command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnSave (object obj)
      {
         var inString = false;
         var formatted = string.Empty;
         foreach (var letter in this.JsonText) {
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
         int a = 0;
      }

      /// <summary>
      /// Verifies if the format command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanFormat (object obj)
      {
         return !string.IsNullOrWhiteSpace(this.CurrentRegex) &&
            !string.IsNullOrWhiteSpace(this.TextToFormat);
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
      /// Occurs when the lights change.
      /// </summary>
      /// <param name="sender">Light collection.</param>
      /// <param name="e">NotifyCollectionChangedEventArgs</param>
      private void OnLightsChanged (object sender, NotifyCollectionChangedEventArgs e)
      {
         var love = GetLove ();
         var url = Constants.URL_GET_TRUTH + HttpUtility.UrlEncode (love);
         WebQueue.Instance.Get (url, this.OnGetTruths);
         if (e.NewItems != null) {
            foreach (var newItem in e.NewItems.Cast <Light>()) {
               newItem.Edit = this;
            }
         }
      }

      /// <summary>
      /// Gets the current love.
      /// </summary>
      /// <returns>The current love.</returns>
      private string GetLove ()
      {
         var love = string.Empty;
         foreach (var light in this.Lights) {
            if (!string.IsNullOrWhiteSpace (love))
               love += ",";
            love += light.Id;
         }
         return love;
      }

      /// <summary>
      /// Occurs when truths have been received from the server.
      /// </summary>
      /// <param name="data">Parameters</param>
      /// <param name="result">Result</param>
      private void OnGetTruths (NameValueCollection data, dynamic result)
      {
         this.Truths.Clear ();
         foreach (var truth in result.truths) {
            this.Truths.Add(new Truth {
               Id = truth.id,
               Order = truth.o,
               Number = truth.n,
               Light = new Light {
                  Id = truth.lid,
                  Text = truth.t
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
         this.Lights.Remove (light);
      }

      /// <summary>
      /// Formats the text to format with the selected regular expression.
      /// </summary>
      private void Format ()
      {
         var startOrder = Convert.ToInt32 (this.StartOrder);
         var startNumber = string.IsNullOrWhiteSpace(this.StartNumber) ? (int?)null : Convert.ToInt32 (this.StartNumber);
         var matches = Regex.Matches (this.TextToFormat, this.CurrentRegex, RegexOptions.IgnoreCase);
         JArray itemList = new JArray ();
         foreach (Match match in matches)
         {
            var order = match.Groups["o"];
            var number = match.Groups["n"];
            var header = match.Groups["h"];
            var item = new JObject {
               ["t"] = match.Groups ["t"].Value.Replace ("\"", "&quot;").Trim (),
               ["o"] = order.Success ? Convert.ToInt32 (order.Value) : startOrder
            };
            if (number.Success || startNumber != null)
               item ["n"] = number.Success ? Convert.ToInt32 (number.Value) : startNumber;
            if (header.Success)
               item ["h"] =header.Value.Replace ("\"", "&quot;").Trim ();
            itemList.Add (item);
            startOrder++;
            if (startNumber.HasValue)
            {
               if (startNumber < 0) startNumber--;
               else startNumber++;
            }
         }
         var blob = new JObject { ["love"] = GetLove (), ["items"] = itemList };
         this.JsonText = blob.ToString ();
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
