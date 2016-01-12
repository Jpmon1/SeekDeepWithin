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
      /// Gets the span of the row.
      /// </summary>
      public int Span { get; private set; }

      /// <summary>
      /// Gets or Sets the starting offset.
      /// </summary>
      public int Offset { get; private set; }

      /// <summary>
      /// Adds a new column for the given item.
      /// </summary>
      /// <param name="span">The span of the item.</param>
      /// <param name="levelItem">The item to add to a column.</param>
      public void AddColumn (int span, LevelItem levelItem)
      {
         this.Span += span;
         var column = new LevelColumn {
            SmallSpan = span == 12 ? 12 : 6,
            LargeSpan = span,
            MediumSpan = span,
            LevelItem = levelItem
         };
         this.Columns.Add (column);
         this.Offset = (12 - span) / 2;
      }
   }
}