using System;
using System.Collections.Generic;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Lays out the given levels.
   /// </summary>
   public class LevelLayout
   {
      private readonly LevelModel m_Level;

      /// <summary>
      /// Initializes a new level layout.
      /// </summary>
      /// <param name="level">The level to layout.</param>
      public LevelLayout (LevelModel level)
      {
         this.Rows = new List <LevelRow> ();
         this.m_Level = level;
         this.Layout ();
      }

      /// <summary>
      /// Gets the list of rows.
      /// </summary>
      public List<LevelRow> Rows { get; private set; }

      /// <summary>
      /// Lays out the given level.
      /// </summary>
      private void Layout ()
      {
         int rowSpan = 0;
         var row = new LevelRow();
         this.Rows.Add (row);
         TruthType lastType = 0;
         foreach (var levelItem in this.m_Level.Items) {
            int span = 3;
            if (levelItem.Type == TruthType.Summary)
               span = 12;
            if (lastType != levelItem.Type || rowSpan == 12) {
               row = new LevelRow ();
               this.Rows.Add (row);
               lastType = levelItem.Type;
            }
         }
      }
   }
}