using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
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
      private RelayCommand m_LoginCommand;
      private RelayCommand m_NewLoveCommand;
      private LoveViewModel m_SelectedLove;
      private bool m_UseProduction;
      private bool m_UseLocal;

      /// <summary>
      /// Propert changed event.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// Initializes a new main view model.
      /// </summary>
      public MainViewModel ()
      {
         this.UseLocal = true;
         this.Light = new LightViewModel ();
         this.Love = new ObservableCollection <LoveViewModel> ();
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
      /// Gets or Sets the selected Love.
      /// </summary>
      public LoveViewModel SelectedLove
      {
         get { return this.m_SelectedLove; }
         set
         {
            this.m_SelectedLove = value;
            this.OnPropertyChanged ();
         }
      }

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
               this.m_UseLocal = value;
               this.OnPropertyChanged ();
               this.UseProduction = !this.m_UseLocal;
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
               this.m_UseProduction = value;
               this.OnPropertyChanged ();
               this.UseLocal = !this.m_UseProduction;
               WebQueue.Instance.UseProduction (this.m_UseProduction);
            }
         }
      }

      /// <summary>
      /// Execution of the new love command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnNewLove (object obj)
      {
         this.Love.Add(new LoveViewModel { IsSelected = true });
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
         var love = new LoveViewModel { IsUpdating = true, IsSelected = true };
         foreach (var light in truth.Love.Peace)
            love.Light.Add (new Light (light));
         love.IsUpdating = false;
         love.Light.Add (truth.Love.Light);
         this.Love.Add (love);
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
