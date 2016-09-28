using System.ComponentModel;
using System.Runtime.CompilerServices;
using IinAll.Edit.Annotations;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// A base view model class.
   /// </summary>
   public class BaseViewModel : INotifyPropertyChanged
   {
      /// <summary>
      /// Property changed event
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// Raises the property changed event.
      /// </summary>
      /// <param name="propertyName">Name of property that changed.</param>
      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
      }
   }
}
