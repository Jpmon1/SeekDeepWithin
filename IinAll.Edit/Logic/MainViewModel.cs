using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using IinAll.Edit.Data;
using IinAll.Edit.Data.Save;
using Peter.Common;
using Peter.Common.Utilities;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// The main view model for IinAll.Edit.
   /// </summary>
   public class MainViewModel : ViewModelBase
   {
      private bool m_UseLocal;
      private string m_UserName;
      private bool m_UseProduction;
      private SaveEnvironment m_SaveData;
      private RelayCommand m_LoginCommand;
      private RelayCommand m_NewLoveCommand;

      /// <summary>
      /// Event occurs when a love is added.
      /// </summary>
      public event EventHandler LoveAdded;

      /// <summary>
      /// Initializes a new main view model.
      /// </summary>
      public MainViewModel ()
      {
         this.UseLocal = true;
         this.Light = new LightViewModel ();
         this.m_SaveData = new SaveEnvironment();
         this.Love = new ObservableCollection <LoveViewModel> ();
         var timer = new DispatcherTimer (DispatcherPriority.Normal, Dispatcher.CurrentDispatcher) {
            Interval = new TimeSpan (0, 1, 0)
         };
         timer.Tick += this.OnTimerTick;
         timer.Start ();
         Instance = this;
      }

      /// <summary>
      /// Gets the main view model instance.
      /// </summary>
      public static MainViewModel Instance { get; private set; }

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
      /// Gets or Sets the password.
      /// </summary>
      public string Password { get; set; }

      /// <summary>
      /// Gets or Sets the light view model.
      /// </summary>
      public LightViewModel Light { get; set; }

      /// <summary>
      /// Gets the collection of Love.
      /// </summary>
      public ObservableCollection <LoveViewModel> Love { get; }

      /// <summary>
      /// Gets the login command.
      /// </summary>
      public ICommand LoginCommand
      {
         get { return this.m_LoginCommand ?? (this.m_LoginCommand = new RelayCommand (this.OnLogin, this.CanLogin)); }
      }

      /// <summary>
      /// Gets the command for a new love.
      /// </summary>
      public ICommand NewLoveCommand
      {
         get { return this.m_NewLoveCommand ?? (this.m_NewLoveCommand = new RelayCommand (this.OnNewLove)); }
      }

      /// <summary>
      /// Gets or Sets if we are using local databases.
      /// </summary>
      public bool UseLocal
      {
         get { return m_UseLocal; }
         set
         {
            if (this.m_UseLocal != value) {
               this.Save ();
               this.m_UseLocal = value;
               this.OnPropertyChanged ();
               this.m_UseProduction = !this.m_UseLocal;
               this.OnPropertyChanged (nameof (UseProduction));
               WebQueue.Instance.UseProduction (this.m_UseProduction);
               this.LoadData ();
            }
         }
      }

      /// <summary>
      /// Gets or Sets if we are using production databases.
      /// </summary>
      public bool UseProduction
      {
         get { return m_UseProduction; }
         set
         {
            if (this.m_UseProduction != value) {
               this.Save ();
               this.m_UseProduction = value;
               this.OnPropertyChanged ();
               this.m_UseLocal = !this.m_UseProduction;
               this.OnPropertyChanged (nameof (UseLocal));
               WebQueue.Instance.UseProduction (this.m_UseProduction);
               this.LoadData ();
            }
         }
      }

      /// <summary>
      /// Execution of the new love command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnNewLove (object obj)
      {
         this.Love.Insert (0, new LoveViewModel { IsExpanded = true });
         this.OnLoveAdded ();
      }

      /// <summary>
      /// Closes the given love.
      /// </summary>
      /// <param name="love">The love to close.</param>
      public void CloseLove (LoveViewModel love)
      {
         this.Love.Remove (love);
      }

      /// <summary>
      /// Edits the love of the given truth.
      /// </summary>
      /// <param name="truth">Truth to edit.</param>
      public void EditTruth (Truth truth)
      {
         var love = new LoveViewModel { IsUpdating = true, IsExpanded = true };
         foreach (var light in truth.Love.Peace)
            love.Light.Add (new Light (light));
         love.IsUpdating = false;
         love.Light.Add (truth.Love.Light);
         this.Love.Insert (0, love);
         this.OnLoveAdded ();
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
      /// Loads any saved data.
      /// </summary>
      public void Load ()
      {
         if (File.Exists (Constants.SAVE_FILE)) {
            this.m_SaveData = Serialization.Deserialize <SaveEnvironment> (Constants.SAVE_FILE);
            if (this.m_SaveData.UseProduction)
               this.UseProduction = true;
            else
               this.LoadData ();
         }
      }

      /// <summary>
      /// Sets the data baseed on the current environment.
      /// </summary>
      private void LoadData ()
      {
         if (this.m_SaveData == null) return;
         var data = this.UseProduction ? this.m_SaveData.ProductionData : this.m_SaveData.LocalData;
         this.UserName = data.User;
         this.Light.SearchText = data.SearchText;
         this.Light.SearchResults.Clear ();
         foreach (var light in data.SearchLight) {
            this.Light.SearchResults.Add (new Light {
               Id = light.Id,
               Text = light.Text
            });
         }
         this.Light.Light.Clear ();
         foreach (var light in data.StagedLight) {
            this.Light.Light.Add (new Light {
               Id = light.Id,
               Text = light.Text
            });
         }

         this.Love.Clear ();
         foreach (var l in data.Love) {
            var love = new LoveViewModel { IsUpdating = true, IsExpanded = l.IsExpanded};
            for (var i = 0; i < l.Light.Count; i++) {
               var light = l.Light [i];
               if (i == l.Light.Count - 1)
                  love.IsUpdating = false;
               love.Light.Add (new Light {
                  Id = light.Id,
                  Text = light.Text
               });
            }
            this.Love.Add (love);
         }
      }

      /// <summary>
      /// Saves the data when the timer ticks.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void OnTimerTick (object sender, EventArgs e)
      {
         this.Save ();
      }

      /// <summary>
      /// Saves the data.
      /// </summary>
      public void Save ()
      {
         if (this.m_SaveData != null) {
            this.m_SaveData.SetData (this);
            this.m_SaveData.Serialize (Constants.SAVE_FILE);
         }
      }

      /// <summary>
      /// Raises the love added event.
      /// </summary>
      private void OnLoveAdded ()
      {
         this.LoveAdded?.Invoke (this, EventArgs.Empty);
      }
   }
}
