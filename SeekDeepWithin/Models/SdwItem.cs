using System.Collections.Generic;
using System.Linq;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// An item found in the level.
   /// </summary>
   public class SdwItem
   {
      /// <summary>
      /// Initializes a new new level item.
      /// </summary>
      public SdwItem ()
      {
         this.Headers = new List<SdwItem> ();
         this.Footers = new List<SdwItem> ();
         this.Styles = new List<SdwStyle> ();
      }

      /// <summary>
      /// Initializes a new new level item.
      /// </summary>
      /// <param name="truth">The truth to use as an item.</param>
      public SdwItem (Truth truth)
      {
         this.Headers = new List<SdwItem> ();
         this.Footers = new List<SdwItem> ();
         this.Styles = new List <SdwStyle> ();
         this.Update(truth);
      }

      /// <summary>
      /// Initializes a new level item from the given light.
      /// </summary>
      /// <param name="light">The light to use.</param>
      public SdwItem (Light light) : this ()
      {
         this.Id = light.Id;
         this.Text = light.Text;
         if (light.Truths.Count == 1) {
            this.Styles.AddRange (light.Truths.First().Styles.Select (s => new SdwStyle (s)));
         }
      }

      /// <summary>
      /// Updates the level item for the given truth.
      /// </summary>
      /// <param name="truth">Truth to use.</param>
      public void Update (Truth truth)
      {
         this.TruthId = truth.Id;
         this.Order = truth.Order;
         this.Number = truth.Number;
         this.ParentId = truth.ParentId;
         if (truth.Light != null) {
            this.Id = truth.Light.Id;
            this.Text = truth.Light.Text;
            this.Styles.AddRange (truth.Styles.Select(s => new SdwStyle (s)));
         }
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
      /// Gets or Sets the key.
      /// </summary>
      public string Key { get; set; }

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
      /// Gets or Sets the truth id for editing.
      /// </summary>
      public int TruthId { get; set; }

      /// <summary>
      /// Gets or Set the parent id hash.
      /// </summary>
      public string Parents { get; set; }

      /// <summary>
      /// Gets or Sets if this item is selected.
      /// </summary>
      public bool IsSelected { get; set; }

      /// <summary>
      /// Gets or Sets the history hash.
      /// </summary>
      public string History { get; set; }

      /// <summary>
      /// Gets or Sets if this is a link or not.
      /// </summary>
      public bool IsLink { get; set; }

      /// <summary>
      /// Gets the list of headers.
      /// </summary>
      public List<SdwItem> Headers { get; private set; }

      /// <summary>
      /// Gets the list of footers.
      /// </summary>
      public List<SdwItem> Footers { get; private set; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      public List<SdwStyle> Styles { get; private set; }

      /// <summary>
      /// Gets or Sets a parent id.
      /// </summary>
      public int? ParentId { get; set; }
   }
}