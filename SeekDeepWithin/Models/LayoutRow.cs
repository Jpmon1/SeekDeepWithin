using System.Collections.Generic;
using System.Linq;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a row for a level.
   /// </summary>
   public class LayoutRow
   {
      /// <summary>
      /// Initializes a new level row.
      /// </summary>
      public LayoutRow ()
      {
         this.Columns = new List <LayoutColumn> ();
      }

      /// <summary>
      /// Gets the list of columns in the row.
      /// </summary>
      public List<LayoutColumn> Columns { get; private set; }

      /// <summary>
      /// Gets the span of the row.
      /// </summary>
      public int Span { get; private set; }

      /// <summary>
      /// Gets or Sets the starting medium offset.
      /// </summary>
      public int MediumOffset { get; private set; }

      /// <summary>
      /// Gets or Sets the starting large offset.
      /// </summary>
      public int LargeOffset { get; private set; }

      /// <summary>
      /// Adds a new column for the given item.
      /// </summary>
      /// <param name="span">The span of the item.</param>
      /// <param name="sdwItem">The item to add to a column.</param>
      public void AddColumn (int span, SdwItem sdwItem)
      {
         this.Span += span;
         var column = new LayoutColumn {
            SmallSpan = sdwItem != null && sdwItem.Type == 0 ? 12 :
                        span == 12 ? 12 : 6,
            LargeSpan = span,
            MediumSpan = span,
            SdwItem = sdwItem
         };
         this.Columns.Add (column);
      }

      /// <summary>
      /// Gets the row ready to display.
      /// </summary>
      public void MakeReady ()
      {
         if (this.Columns.Count == 1 && this.Columns [0].LargeSpan == 3) {
            this.Columns [0].LargeSpan = 4;
            this.Columns [0].MediumSpan = 6;
         } else if (this.Columns.Count == 3) {
            if (this.Columns.All (c => c.LargeSpan == 3)) {
               foreach (var column in Columns) {
                  column.LargeSpan = 4;
                  column.MediumSpan = 4;
               }
            }
         }
         this.LargeOffset = (12 - this.Columns.Sum (c => c.LargeSpan)) / 2;
         this.MediumOffset = (12 - this.Columns.Sum (c => c.MediumSpan)) / 2;
      }
   }
}