using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using IinAll.Edit.Data;
using Newtonsoft.Json.Linq;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// View model for bliss.
   /// </summary>
   public class BlissViewModel : BaseTabItem, ILightContainer
   {
      private string m_StartOrder;
      private string m_StartNumber;

      /// <summary>
      /// Initializes a new bliss view model.
      /// </summary>
      public BlissViewModel ()
      {
         this.Title = "Bliss";
         this.Light = new ObservableCollection<Light> ();
         this.Bliss = new ObservableCollection<Bliss> ();
         this.Light.CollectionChanged += this.OnLightChanged;
      }

      /// <summary>
      /// Gets the list of staged light.
      /// </summary>
      public ObservableCollection<Light> Light { get; }

      /// <summary>
      /// Gets the collection of bliss.
      /// </summary>
      public ObservableCollection<Bliss> Bliss { get; private set; }

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
      /// Removes the given light.
      /// </summary>
      /// <param name="light">Light to remove.</param>
      public void RemoveLight (Light light)
      {
         this.Light.Remove (light);
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
      }

      /// <summary>
      /// Gets the current love.
      /// </summary>
      /// <returns>The current love.</returns>
      private string GetLove ()
      {
         var love = string.Empty;
         foreach (var light in this.Light) {
            if (!string.IsNullOrWhiteSpace (love))
               love += ",";
            love += light.Id;
         }
         return love;
      }
   }
}
