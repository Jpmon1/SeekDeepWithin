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
      private bool m_IsWaiting;
      private string m_UserName;
      private RelayCommand m_LoginCommand;
      private RelayCommand m_NewTruthCommand;
      private RelayCommand m_NewBlissCommand;
      private TruthViewModel m_SelectedTruth;
      private BlissViewModel m_SelectedBliss;

      /// <summary>
      /// Propert changed event.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// Initializes a new main view model.
      /// </summary>
      public MainViewModel ()
      {
         this.Light = new LightViewModel ();
         this.Truth = new ObservableCollection <TruthViewModel> ();
         this.Bliss = new ObservableCollection <BlissViewModel> ();
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
      /// Gets or Sets the password.
      /// </summary>
      public string Password { get; set; }

      /// <summary>
      /// Gets or Sets the light view model.
      /// </summary>
      public LightViewModel Light { get; set; }

      /// <summary>
      /// Gets the collection of truth.
      /// </summary>
      public ObservableCollection <TruthViewModel> Truth { get; }

      /// <summary>
      /// Gets the collection of bliss.
      /// </summary>
      public ObservableCollection <BlissViewModel> Bliss { get; }

      /// <summary>
      /// Gets or Sets the selected truth.
      /// </summary>
      public TruthViewModel SelectedTruth
      {
         get { return this.m_SelectedTruth; }
         set
         {
            this.m_SelectedTruth = value;
            this.OnPropertyChanged ();
         }
      }

      /// <summary>
      /// Gets or Sets the selected biss.
      /// </summary>
      public BlissViewModel SelectedBliss
      {
         get { return this.m_SelectedBliss; }
         set
         {
            this.m_SelectedBliss = value;
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
      /// Gets the command for a new truth.
      /// </summary>
      public ICommand NewTruthCommand
      {
         get { return this.m_NewTruthCommand ?? (this.m_NewTruthCommand = new RelayCommand (this.OnNewTruth)); }
      }

      /// <summary>
      /// Gets the command for a new bliss.
      /// </summary>
      public ICommand NewBlissCommand
      {
         get { return this.m_NewBlissCommand ?? (this.m_NewBlissCommand = new RelayCommand (this.OnNewBliss)); }
      }

      /// <summary>
      /// Execution of the new truth command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnNewTruth (object obj)
      {
         this.Truth.Add(new TruthViewModel { IsSelected = true });
      }

      /// <summary>
      /// Execution of the new truth command.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnNewBliss (object obj)
      {
         this.Bliss.Add (new BlissViewModel { IsSelected = true });
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
            !WebQueue.Instance.IsAuthenticated &&
            !this.m_IsWaiting;
      }

      /// <summary>
      /// Performs the login routine.
      /// </summary>
      /// <param name="obj">Command Parameter, not used.</param>
      private void OnLogin (object obj)
      {
         byte[] passHash;
         this.m_IsWaiting = true;
         var data = Encoding.UTF8.GetBytes (this.Password);
         using (SHA512 shaM = new SHA512Managed ())
            passHash = shaM.ComputeHash (data);
         var values = new NameValueCollection {
            {"email", this.UserName},
            {"hash", BitConverter.ToString (passHash).Replace("-", "").ToLower ()}
         };
         WebQueue.Instance.Post (Constants.URL_LOGIN_REQUEST, values, (d, r) => this.m_IsWaiting = false);
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
