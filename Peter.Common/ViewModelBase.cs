/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Peter.Common
{
   /// <summary>
   /// Base class for any view model.
   /// </summary>
   public abstract class ViewModelBase : INotifyPropertyChanged
   {
      private readonly Dispatcher m_GuiDispatcher;

      /// <summary>
      /// View Model Base constructor.
      /// </summary>
      protected ViewModelBase ()
      {
         this.m_GuiDispatcher = Dispatcher.CurrentDispatcher;
      }

      /// <summary>
      /// Get the GUI Dipatcher.
      /// </summary>
      [XmlIgnore]
      public Dispatcher GuiDispatcher { get { return m_GuiDispatcher; } }

      /// <summary>
      /// Gets or Sets if this item has been composed by MEF or not.
      /// </summary>
      [XmlIgnore]
      public bool IsComposed { get; set; }

      /// <summary>
      /// Raised when a property on this object has a new value.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// Raises this object's PropertyChanged event.
      /// </summary>
      /// <param name="propertyName">The property that has a new value.</param>
      protected void OnPropertyChanged ([CallerMemberName] string propertyName = null)
      {
         this.PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
      }
   }
}
