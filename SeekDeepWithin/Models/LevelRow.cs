using System.Collections.Generic;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a row for a level.
   /// </summary>
   public class LevelRow
   {
      /// <summary>
      /// Initializes a new level row.
      /// </summary>
      public LevelRow ()
      {
         this.Columns = new List <LevelColumn> ();
      }

      /// <summary>
      /// Gets the list of columns in the row.
      /// </summary>
      public List<LevelColumn> Columns { get; private set; }

      /// <summary>
      /// Gets or Sets the starting offset for medium screens.
      /// </summary>
      public int MediumOffset { get; set; }

      /// <summary>
      /// Gets or Sets the starting offset for large screens.
      /// </summary>
      public int LargeOffset { get; set; }
   }
}