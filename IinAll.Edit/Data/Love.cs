using System.Collections.ObjectModel;
using System.Collections.Specialized;
using IinAll.Edit.Logic;

namespace IinAll.Edit.Data
{
   /// <summary>
   /// Model for love.
   /// </summary>
   public class Love : ILightContainer
   {
      /// <summary>
      /// Initializes a new love.
      /// </summary>
      public Love ()
      {
         this.Peace = new ObservableCollection <Light> ();
         this.Peace.CollectionChanged += this.OnPeaceChanged;
      }

      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Get or Sets the light.
      /// </summary>
      public Light Light { get; set; }

      /// <summary>
      /// Gets or Sets the peace.
      /// </summary>
      public ObservableCollection <Light> Peace { get; }

      /// <summary>
      /// Occurs when the peace changes.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void OnPeaceChanged (object sender, NotifyCollectionChangedEventArgs e)
      {
         foreach (Light light in e.NewItems) {
            light.Parent = this;
         }
      }

      /// <summary>
      /// Removes the given light.
      /// </summary>
      /// <param name="light">Light to remove.</param>
      /// <param name="type"></param>
      public void RemoveLight (Light light, LightType type)
      {
         this.Peace.Remove (light);
      }
   }
}
