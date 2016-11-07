using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Input;
using IinAll.Edit.Data;
using IinAll.Edit.Interfaces;
using IinAll.Edit.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Peter.Common;
using Style = IinAll.Edit.Data.Style;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// View model for truth.
   /// </summary>
   public class LoveViewModel : BaseTabItem, ILightContainer, IBodyContainer, IStyleContainer
   {
      private Light m_Text;
      private int m_EndIndex;
      private int m_StartIndex;
      private string m_AliasId;
      private int m_EditLoveId;
      private int m_LoveId = -1;
      private int? m_StartNumber;
      private string m_TextToFormat;
      private bool m_AppendFormattedText;
      private RelayCommand m_CmdSave;
      private RelayCommand m_CmdRemove;
      private RelayCommand m_CmdFormat;
      private RelayCommand m_CloseCommand;
      private RelayCommand m_MoveUpCommand;
      private RelayCommand m_AddBodyCommand;
      private RelayCommand m_AddStyleCommand;
      private RelayCommand m_MoveDownCommand;
      private RelayCommand m_AddEditLightCommand;

      /// <summary>
      /// Initializes a new truth view model.
      /// </summary>
      public LoveViewModel ()
      {
         this.Title = "Love";
         this.ToolTip = "Love";
         this.Light = new ObservableCollection <Light> ();
         this.Truth = new ObservableCollection <Truth> ();
         this.Bodies = new ObservableCollection <Body> ();
         this.Styles = new ObservableCollection <Style> ();
         this.EditLight = new ObservableCollection <Light> ();
         this.Light.CollectionChanged += this.OnLightChanged;
         this.Truth.CollectionChanged += this.OnTruthChanged;
         this.EditLight.CollectionChanged += this.OnEditLightChanged;
         this.Regexes = new ObservableCollection<string> ();
         if (Settings.Default.RegExes != null) {
            foreach (var regEx in Settings.Default.RegExes)
               this.Regexes.Add (regEx);
         }
      }

      /// <summary>
      /// Gets or Sets the text light.
      /// </summary>
      public Light Text
      {
         get { return m_Text; }
         private set
         {
            if (this.m_Text != value) {
               m_Text = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Gets the list of staged light.
      /// </summary>
      public ObservableCollection<Light> Light { get; }

      /// <summary>
      /// Gets the list of staged light.
      /// </summary>
      public ObservableCollection<Light> EditLight { get; }

      /// <summary>
      /// Gets the collection of truth.
      /// </summary>
      public ObservableCollection<Truth> Truth { get; }

      /// <summary>
      /// Gets the current collection of RegExes.
      /// </summary>
      public ObservableCollection<string> Regexes { get; }

      /// <summary>
      /// Gets the list of bodies.
      /// </summary>
      public ObservableCollection<Body> Bodies { get; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      public ObservableCollection<Style> Styles { get; }

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
      /// Gets or Sets the number.
      /// </summary>
      public int? StartNumber
      {
         get { return this.m_StartNumber; }
         set
         {
            if (this.m_StartNumber != value)
            {
               this.m_StartNumber = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Gets or Sets the love id.
      /// </summary>
      public int LoveId
      {
         get { return this.m_LoveId; }
         set
         {
            if (this.m_LoveId != value)
            {
               this.m_LoveId = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Gets or Sets if this is updating so we should not perform server calls.
      /// </summary>
      public bool IsUpdating { get; set; }

      /// <summary>
      /// Gets the id of the love for the edit light.
      /// </summary>
      public int EditLoveId
      {
         get { return this.m_EditLoveId; }
         private set
         {
            if (this.m_EditLoveId != value) {
               this.m_EditLoveId = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Gets or Sets the alias id.
      /// </summary>
      public string AliasId
      {
         get { return this.m_AliasId; }
         set
         {
            if (this.m_AliasId != value)
            {
               this.m_AliasId = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Gets or Sets if the formatted text should be appended or overwritten.
      /// </summary>
      public bool AppendFormattedText
      {
         get { return this.m_AppendFormattedText; }
         set
         {
            if (this.m_AppendFormattedText != value) {
               this.m_AppendFormattedText = value;
               this.OnPropertyChanged ();
            }
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
      /// Gets the remove command.
      /// </summary>
      public ICommand RemoveCommand
      {
         get { return this.m_CmdRemove ?? (this.m_CmdRemove = new RelayCommand (this.OnRemove, this.CanFormat)); }
      }

      /// <summary>
      /// Gets the save command.
      /// </summary>
      public ICommand SaveCommand
      {
         get { return this.m_CmdSave ?? (this.m_CmdSave = new RelayCommand (this.OnSave, this.CanSave)); }
      }

      /// <summary>
      /// Gets the command to move truth up.
      /// </summary>
      public ICommand MoveUpCommand
      {
         get { return this.m_MoveUpCommand ?? (this.m_MoveUpCommand = new RelayCommand (this.OnMoveUp, this.CanMove)); }
      }

      /// <summary>
      /// Gets the command to move truth down.
      /// </summary>
      public ICommand MoveDownCommand
      {
         get { return this.m_MoveDownCommand ?? (this.m_MoveDownCommand = new RelayCommand (this.OnMoveDown, this.CanMove)); }
      }

      /// <summary>
      /// Gets the command to close this tab.
      /// </summary>
      public ICommand CloseCommand
      {
         get { return this.m_CloseCommand ?? (this.m_CloseCommand = new RelayCommand (this.OnClose, this.CanClose)); }
      }

      /// <summary>
      /// Gets the command to add the edit light.
      /// </summary>
      public ICommand AddEditLightCommand
      {
         get { return this.m_AddEditLightCommand ?? (this.m_AddEditLightCommand = new RelayCommand (this.OnAddEditLight, this.CanAddEditLight)); }
      }

      /// <summary>
      /// Gets the command to add a new body.
      /// </summary>
      public ICommand AddBodyCommand
      {
         get { return this.m_AddBodyCommand ?? (this.m_AddBodyCommand = new RelayCommand (this.OnAddBody, this.CanAddBody)); }
      }

      /// <summary>
      /// Gets the command to add a new style.
      /// </summary>
      public ICommand AddStyleCommand
      {
         get { return this.m_AddStyleCommand ?? (this.m_AddStyleCommand = new RelayCommand (this.OnAddStyle, this.CanAddBody)); }
      }

      /// <summary>
      /// Gets or Sets the start index.
      /// </summary>
      public int StartIndex
      {
         get { return this.m_StartIndex; }
         set
         {
            if (!this.m_StartIndex.Equals (value)) {
               this.m_StartIndex = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Gets or Sets the end index.
      /// </summary>
      public int EndIndex
      {
         get { return this.m_EndIndex; }
         set
         {
            if (!this.m_EndIndex.Equals (value)) {
               this.m_EndIndex = value;
               this.OnPropertyChanged ();
            }
         }
      }

      /// <summary>
      /// Verifies if the add body command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanAddBody (object obj)
      {
         return this.LoveId > 0;
      }

      /// <summary>
      /// Executes the add body command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnAddBody (object obj)
      {
         this.Bodies.Add(new Body (this, this.LoveId));
      }

      /// <summary>
      /// Executes the add style command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnAddStyle (object obj)
      {
         this.Styles.Add (new Style (this, this.LoveId) { StartIndex = this.StartIndex, EndIndex = this.EndIndex});
      }

      /// <summary>
      /// Verifies if the move command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanMove (object obj)
      {
         return this.Truth.Any (t => t.IsSelected);
      }

      /// <summary>
      /// Executes the move up command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnMoveUp (object obj)
      {
         for (int a = 0; a < this.Truth.Count; a++) {
            var truth = this.Truth [a];
            if (truth.IsSelected && a > 0) {
               this.Truth.Remove (truth);
               this.Truth.Insert (a - 1, truth);
            }
         }
      }

      /// <summary>
      /// Executes the move down command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnMoveDown (object obj)
      {
         for (int a = this.Truth.Count - 1; a >= 0; a--) {
            var truth = this.Truth[a];
            if (truth.IsSelected && a < this.Truth.Count - 1) {
               this.Truth.Remove (truth);
               this.Truth.Insert (a + 1, truth);
            }
         }
      }

      /// <summary>
      /// Verifies if the add edit light command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanAddEditLight (object obj)
      {
         return this.EditLight.Count > 0;
      }

      /// <summary>
      /// Executes the add edit light command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnAddEditLight (object obj)
      {
         var love = new Love {
            Light = new Light {
               Id = this.EditLight.Last ().Id,
               Text = this.EditLight.Last ().Text
            }
         };
         for (var a = 0; a < this.EditLight.Count - 1; a++) {
            love.Peace.Add(new Light {
               Id = this.EditLight[a].Id,
               Text = this.EditLight[a].Text
            });
         }
         this.Truth.Add(new Truth (this) {
            Love = love,
            IsNew = true,
            Order = !this.Truth.Any () ? 1 : this.Truth.Max(l => l.Order) + 1
         });
      }

      /// <summary>
      /// Verifies if the close command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanClose (object obj)
      {
         return true; // TODO: Don't close if processing data...
      }

      /// <summary>
      /// Executes the close command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnClose (object obj)
      {
         MainViewModel.Instance.CloseLove (this);
      }

      /// <summary>
      /// Verifies if the save command can execute.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      /// <returns>True if we can, otherwise false.</returns>
      private bool CanSave (object obj)
      {
         return this.Light.Count > 0 && WebQueue.Instance.IsAuthenticated && this.Truth.Count > 0;
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
         /*foreach (var letter in this.JsonText)
         {
            if (letter == '\r' || letter == '\n') continue;
            if (letter == ' ' && !inString) continue;
            if (letter == '"') inString = !inString;
            formatted += letter;
         }*/
         this.SaveNewTruth ();
         if (this.LoveId > 0)
            this.SaveModifiedTruth ();
      }

      /// <summary>
      /// Saves all new truth.
      /// </summary>
      private void SaveNewTruth ()
      {
         var truth = new JArray ();
         foreach (var t in this.Truth.Where (tr => tr.IsNew)) {
            var tObj = new JObject {
               ["order"] = t.Order,
               ["love"] = t.GetLove (),
               ["alias"] = t.Alias
            };
            if (t.Bodies.Count > 0) {
               var bodies = new JArray ();
               foreach (var body in t.Bodies) {
                  bodies.Add (new JObject {
                     ["pos"] = body.GetPosition (),
                     ["text"] = body.Text
                  });
               }
               tObj ["body"] = bodies;
            }
            truth.Add (tObj);
         }
         if (truth.Count > 0) {
            var data = new JObject {["love"] = this.GetLove (), ["truth"] = truth};
            var parameters = new NameValueCollection { {"d", data.ToString (Formatting.None)} };
            WebQueue.Instance.Post (Constants.URL_TRUTH, parameters, this.OnSaveSuccess);
         }
      }

      /// <summary>
      /// Saves all modified truth.
      /// </summary>
      private void SaveModifiedTruth ()
      {
         var truth = new JArray ();
         foreach (var t in this.Truth.Where (tr => !tr.IsNew && tr.IsModified)) {
            truth.Add (new JObject {
               ["order"] = t.Order,
               ["truthid"] = t.Id
            });
         }
         if (truth.Count > 0) {
            var data = new JObject { ["truth"] = truth };
            var parameters = new NameValueCollection { {"d", data.ToString (Formatting.None)} };
            WebQueue.Instance.Put (Constants.URL_TRUTH, parameters, this.OnSaveModifiedSuccess);
         }
      }

      /// <summary>
      /// Occurs after the save modified request has returned.
      /// </summary>
      /// <param name="parameters">The original parameters.</param>
      /// <param name="result">The result from the server.</param>
      private void OnSaveModifiedSuccess (NameValueCollection parameters, dynamic result)
      {
         foreach (var truth in this.Truth) {
            truth.IsModified = false;
         }
      }

      /// <summary>
      /// Occurs when the search method has returned.
      /// </summary>
      /// <param name="data">The data passed to the search.</param>
      /// <param name="result">The result of the search.</param>
      private void OnSaveSuccess (NameValueCollection data, dynamic result)
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
         this.SaveRegex ();
         this.Format ();
      }

      /// <summary>
      /// Saves the current regex if not saved.
      /// </summary>
      private void SaveRegex ()
      {
         if (!this.Regexes.Contains (this.CurrentRegex)) {
            this.Regexes.Add (this.CurrentRegex);
            Settings.Default.RegExes.Add (this.CurrentRegex);
            Settings.Default.Save ();
         }
      }

      /// <summary>
      /// Executes the remove command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnRemove (object obj)
      {
         this.SaveRegex ();
         this.TextToFormat = Regex.Replace (this.TextToFormat, this.CurrentRegex, "", RegexOptions.IgnoreCase);
      }

      /// <summary>
      /// Occurs when the edit light changes.
      /// </summary>
      /// <param name="sender">The collection</param>
      /// <param name="e">NotifyCollectionChangedEventArgs</param>
      private void OnEditLightChanged (object sender, NotifyCollectionChangedEventArgs e)
      {
         if (e.NewItems != null) {
            foreach (var light in e.NewItems.Cast <Light> ()) {
               light.Parent = this;
               light.Type = LightType.Edit;
            }
         }
         var love = GetEditLove();
         if (!string.IsNullOrWhiteSpace (love)) {
            var url = Constants.URL_GET_LOVE + HttpUtility.UrlEncode (love);
            WebQueue.Instance.Get (url, this.OnGetEditLove);
         }
      }

      /// <summary>
      /// Results from the get love.
      /// </summary>
      /// <param name="args">The arguments passed.</param>
      /// <param name="result">The result.</param>
      private void OnGetEditLove (NameValueCollection args, dynamic result)
      {
         this.EditLoveId = result.love;
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
         if (this.Light.Count > 0) {
            var last = this.Light.Last ();
            this.Text = new Light (last);
            this.Title = last.Text.Substring(0, Math.Min (50, last.Text.Length));
            var toolTip = string.Empty;
            foreach (var light in this.Light) {
               if (!string.IsNullOrWhiteSpace (toolTip))
                  toolTip += "|";
               toolTip += light.Text;
            }
            this.ToolTip = toolTip;
         } else {
            this.Title = "Love";
            this.ToolTip = "Love";
         }
         if (!this.IsUpdating)
            this.GetTruth();
      }

      /// <summary>
      /// Occurs when the list of truth changes.
      /// </summary>
      /// <param name="sender">The list of truth.</param>
      /// <param name="e">NotifyCollectionChangedEventArgs</param>
      private void OnTruthChanged (object sender, NotifyCollectionChangedEventArgs e)
      {
         var order = 0;
         foreach (var truth in this.Truth) {
            truth.Order = ++order;
         }
      }

      /// <summary>
      /// Gets the truths for the current light.
      /// </summary>
      private void GetTruth ()
      {
         this.LoveId = -1;
         this.Truth.Clear ();
         this.Bodies.Clear ();
         var love = GetLove ();
         this.AliasId = "Loading...";
         if (!string.IsNullOrWhiteSpace (love)) {
            var url = Constants.URL_GET_TRUTH + HttpUtility.UrlEncode (love);
            WebQueue.Instance.Get (url, this.OnGetTruths);
         }
      }

      /// <summary>
      /// Returns the truths for the current love.
      /// </summary>
      /// <param name="parameters">parameters</param>
      /// <param name="result">results</param>
      private void OnGetTruths (NameValueCollection parameters, dynamic result)
      {
         if (result.love == -1) return;
         this.LoveId = result.love;
         var url = Constants.URL_GET_BODY + this.LoveId;
         WebQueue.Instance.Get (url, this.OnGetBodies);
         url = Constants.URL_GET_STYLE + this.LoveId;
         WebQueue.Instance.Get (url, this.OnGetStyles);
         url = Constants.URL_GET_ALIAS + this.LoveId;
         WebQueue.Instance.Get (url, this.OnGetAlias);
         foreach (var t in result.truths) {
            var lights = new List <Light> ();
            foreach (var light in t.Value.l) {
               lights.Add (new Light { Id = light.id, Text = light.text });
            }
            var love = new Love {
               Id = t.Value.id,
               Light = lights.Last ()
            };
            for (var a = 0; a < lights.Count - 1; a++) {
               love.Peace.Add (lights [a]);
            }
            var truth = new Truth (this, Convert.ToInt32 (t.Value.tid)) {
               Order = t.Value.order,
               Love = love,
               IsModified = false
            };
            if (t.Value.a != null) {
               truth.Alias = Convert.ToInt32 (t.Value.a);
            }
            if (t.Value.b != null) {
               foreach (var body in t.Value.b) {
                  truth.Bodies.Add (new Body(truth, Convert.ToInt32 (t.Value.id)) {
                     Id = body.id,
                     Text = body.text,
                     Position = body.position
                  });
               }
            }
            this.Truth.Add (truth);
         }
      }

      /// <summary>
      /// Returns the alias for the current love.
      /// </summary>
      /// <param name="parameters">parameters</param>
      /// <param name="result">results</param>
      private void OnGetAlias (NameValueCollection parameters, dynamic result)
      {
         // TODO:
         this.AliasId = result.alias == -1 ? "None" : result.alias;
      }

      /// <summary>
      /// Returns the body for the current love.
      /// </summary>
      /// <param name="parameters">parameters</param>
      /// <param name="result">results</param>
      private void OnGetBodies (NameValueCollection parameters, dynamic result)
      {
         foreach (var body in result.body) {
            this.Bodies.Add(new Body(this, this.LoveId) {
               Id = body.id,
               Position = body.position,
               Text = body.text
            });
         }
      }

      /// <summary>
      /// Returns the style for the current love.
      /// </summary>
      /// <param name="parameters">parameters</param>
      /// <param name="result">results</param>
      private void OnGetStyles (NameValueCollection parameters, dynamic result)
      {
         foreach (var style in result.style) {
            this.Styles.Add (new Style (this, this.LoveId) {
               Id = style.id,
               StartIndex = style.start,
               EndIndex = style.end,
               Tag = style.tag,
               Css = style.css
            });
         }
      }

      /// <summary>
      /// Removes the given light.
      /// </summary>
      /// <param name="light">Light to remove.</param>
      /// <param name="type"></param>
      public void RemoveLight (Light light, LightType type)
      {
         if (type == LightType.Edit)
            this.EditLight.Remove (light);
         else
            this.Light.Remove (light);
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
      /// Removes a style from the styles.
      /// </summary>
      /// <param name="style">The style to remove.</param>
      public void RemoveStyle (Style style)
      {
         this.Styles.Remove (style);
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
      /// Gets the current edit love.
      /// </summary>
      /// <returns>The current love.</returns>
      private string GetEditLove ()
      {
         var love = string.Empty;
         foreach (var light in this.EditLight)
         {
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
         var number = this.StartNumber;
         if (!this.AppendFormattedText) {
            for (var i = this.Truth.Count - 1; i >= 0; i--) {
               if (this.Truth[i].Id <= 0)
                  this.Truth.RemoveAt (i);
            }
         }
         var matches = Regex.Matches (this.TextToFormat, this.CurrentRegex, RegexOptions.IgnoreCase);
         foreach (Match match in matches) {
            var numberGroup = match.Groups ["n"];
            var headerGroup = match.Groups ["h"];
            var footerGroup = match.Groups ["f"];
            if (numberGroup.Success)
               number = Convert.ToInt32 (numberGroup.Value);
            var text = match.Groups ["t"].Value.Trim ();
            var truth = new Truth (this) {
               IsNew = true,
               Love = new Love { Light = new Light { Text = text } }
            };
            if (numberGroup.Success || number.HasValue) {
               truth.Bodies.Add (new Body (truth) {
                  BodyType = BodyType.Left,
                  Text = number.ToString ()
               });
            }
            if (headerGroup.Success) {
               truth.Bodies.Add (new Body (truth) {
                  BodyType = BodyType.Header,
                  Text = headerGroup.Value
               });
            }
            if (footerGroup.Success) {
               truth.Bodies.Add (new Body (truth) {
                  BodyType = BodyType.Footer,
                  Text = footerGroup.Value
               });
            }
            foreach (var light in this.Light) {
               truth.Love.Peace.Add (new Light (light));
            }
            this.Truth.Add(truth);
            if (!numberGroup.Success && number.HasValue)
               number++;
         }
      }

      /// <summary>
      /// Removes the given truth from the list.
      /// </summary>
      /// <param name="truth">The truth to remove.</param>
      internal void RemoveTruth (Truth truth)
      {
         this.Truth.Remove (truth);
      }
   }
}
