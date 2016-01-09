using System.Collections.Generic;
using System.Linq;
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
      public LevelItem () { }

      /// <summary>
      /// Initializes a new new level item.
      /// </summary>
      /// <param name="loveId">The id of the parent love (for grouping).</param>
      /// <param name="truth">The truth to use as an item.</param>
      public LevelItem (int loveId, Truth truth)
      {
         this.LoveId = loveId;
         this.TruthId = truth.Id;
         this.Id = truth.Light.Id;
         this.Order = truth.Order;
         this.Number = truth.Number;
         this.Text = truth.Light.Text;
         this.Type = (TruthType)truth.Type;
      }

      /// <summary>
      /// Gets or Sets the id of the parent love.
      /// </summary>
      public int LoveId { get; set; }

      /// <summary>
      /// Gets or Sets the parent level.
      /// </summary>
      public LevelModel Level { get; set; }

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
      public TruthType Type { get; set; }

      /// <summary>
      /// Gets or Sets if the item is selected or not.
      /// </summary>
      public bool IsSelected { get; set; }

      /// <summary>
      /// Gets or Sets if we want to show all connections or not.
      /// </summary>
      public bool ShowAll { get; set; }

      /// <summary>
      /// Gets or Sets the truth id for editing.
      /// </summary>
      public int TruthId { get; set; }

      /// <summary>
      /// Gets the list of parent selections.
      /// </summary>
      public List <int> Selection { get; private set; }

      /// <summary>
      /// Sets the list of parent selections
      /// </summary>
      public void SetSelection ()
      {
         var sel = new List <int> ();
         if (this.IsSelected)
            sel.Add (this.Id);
         var level = this.Level.Previous;
         while (level != null) {
            sel.AddRange (from levelItem in level.Items where levelItem.IsSelected select levelItem.Id);
            level = level.Previous;
         }
         this.Selection = sel;
      }
   }
}