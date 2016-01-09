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
   }
}