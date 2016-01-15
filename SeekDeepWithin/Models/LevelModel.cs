using System.Collections.ObjectModel;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a level on SDW.
   /// </summary>
   public class LevelModel
   {
      /// <summary>
      /// Initializes a new level.
      /// </summary>
      public LevelModel ()
      {
         this.Items = new Collection <LevelItem> ();
      }

      /// <summary>
      /// Gets or Sets the previous level.
      /// </summary>
      public LevelModel Previous { get; set; }

      /// <summary>
      /// Get the items on this level.
      /// </summary>
      public Collection<LevelItem> Items { get; private set; }

      /// <summary>
      /// Gets or Sets the next level.
      /// </summary>
      public LevelModel Next { get; set; }

      /// <summary>
      /// Get or Sets the index of the level.
      /// </summary>
      public int Index { get; set; }

      /// <summary>
      /// Gets or Sets the key for this level stored in the database.
      /// </summary>
      public int Key { get; set; }

      /// <summary>
      /// Gets the key to the parent model.
      /// </summary>
      public int Parent { get; set; }

      /// <summary>
      /// Gets or Sets the key this level should replace.
      /// </summary>
      public int Replace { get; set; }

      /// <summary>
      /// Gets the data to save to the database.
      /// </summary>
      /// <returns>The string representation of the data in this level.</returns>
      public string GetData ()
      {
         var data = string.Empty;
         foreach (var item in this.Items) {
            if (!string.IsNullOrEmpty (data))
               data += "|";
            data += string.Format ("{0}.{1}.{2}", item.Id, item.LoveId, item.TruthId);
         }
         return data;
      }
   }
}