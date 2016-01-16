using System.Collections.Generic;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// An item found in the level.
   /// </summary>
   public class LevelItem
   {
      /// <summary>
      /// Initializes a new new level item.
      /// </summary>
      public LevelItem ()
      {
         this.Headers = new List<LevelItem> ();
         this.Footers = new List<LevelItem> ();
      }

      /// <summary>
      /// Initializes a new new level item.
      /// </summary>
      /// <param name="truth">The truth to use as an item.</param>
      public LevelItem (Truth truth)
      {
         this.Headers = new List<LevelItem> ();
         this.Footers = new List<LevelItem> ();
         this.Update(truth);
      }

      /// <summary>
      /// Initializes a new level item from the given light.
      /// </summary>
      /// <param name="light">The light to use.</param>
      public LevelItem (Light light) : this ()
      {
         this.Id = light.Id;
         this.Text = light.Text;
      }

      /// <summary>
      /// Updates the level item for the given truth.
      /// </summary>
      /// <param name="truth">Truth to use.</param>
      public void Update (Truth truth)
      {
         this.TruthId = truth.Id;
         this.Id = truth.Light.Id;
         this.Order = truth.Order;
         this.Number = truth.Number;
         this.Text = truth.Light.Text;
         this.Type = (SdwType) truth.Type;
      }

      public string Title { get; set; }

      /// <summary>
      /// Gets or Sets the id of the parent love.
      /// </summary>
      public int LoveId { get; set; }

      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the order.
      /// </summary>
      public int? Order { get; set; }

      /// <summary>
      /// Gets or Sets the number.
      /// </summary>
      public int? Number { get; set; }

      /// <summary>
      /// Gets or Sets the type
      /// </summary>
      public SdwType Type { get; set; }

      /// <summary>
      /// Gets or Sets the truth id for editing.
      /// </summary>
      public int TruthId { get; set; }

      /// <summary>
      /// Gets or Set the parent id hash.
      /// </summary>
      public string Parents { get; set; }

      /// <summary>
      /// Gets the list of headers.
      /// </summary>
      public List<LevelItem> Headers { get; private set; }

      /// <summary>
      /// Gets the list of footers.
      /// </summary>
      public List<LevelItem> Footers { get; private set; }
   }
}