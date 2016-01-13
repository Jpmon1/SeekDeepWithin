using System;
using System.Collections.Generic;
using System.Linq;
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
         this.Rows.Clear ();
         var loveIds = new List <int> ();
         foreach (var li in this.m_Level.Items) {
            if (!loveIds.Contains (li.LoveId))
               loveIds.Add (li.LoveId);
         }
         var loves = loveIds.OrderBy(i => i).Select (id => this.m_Level.Items.OrderBy (i => i.Order)
            .Where (i => i.LoveId == id).ToList ()).ToList ();
         SetHeadersAndFooters (loves);
         var span = 3;
         var loveCount = loveIds.Count;
         if (loveCount > 1) {
            span = 12 / loveIds.Count;
         }
         if (span < 3) span = 3;
         AddColumns (loves, 0, 4, loveCount);
         var truthTypes = Enum.GetValues (typeof (TruthType)).Cast<TruthType> ();
         foreach (var type in truthTypes) {
            AddColumns (loves, type, span, loveCount);
         }
         for (var i = this.Rows.Count - 1; i >= 0; i--) {
            if (this.Rows[i].Columns.Count <= 0)
               this.Rows.RemoveAt (i);
         }
      }

      /// <summary>
      /// Adds columns for the given truth type.
      /// </summary>
      /// <param name="loves">Loves to look in.</param>
      /// <param name="truthType">Truth type to find.</param>
      /// <param name="span">The suggested span.</param>
      /// <param name="loveCount">The number of loves.</param>
      private void AddColumns (IEnumerable <List <LevelItem>> loves, TruthType truthType, int span, int loveCount)
      {
         var row = new LevelRow ();
         this.Rows.Add (row);
         foreach (var love in loves) {
            foreach (var li in love.Where (li => li.Type == truthType)) {
               var cSpan = span;
               if (loveCount == 1 && (truthType == TruthType.Summary || truthType == TruthType.Passage)) {
                  cSpan = 12;
               }
               if (row.Span + cSpan > 12) {
                  row = new LevelRow ();
                  this.Rows.Add (row);
               }
               row.AddColumn (cSpan, li);
            }
         }
      }

      /// <summary>
      /// Sets the headers and footers.
      /// </summary>
      /// <param name="loves">The list of loves.</param>
      private static void SetHeadersAndFooters (IEnumerable <List <LevelItem>> loves)
      {
         foreach (var love in loves) {
            var remove = new List <LevelItem> ();
            foreach (var header in love.Where (li => li.Type == TruthType.Header)) {
               var passage = love.FirstOrDefault (li => li.Number == header.Number);
               if (passage != null) {
                  passage.Headers.Add (header);
               } else {
                  love.First ().Headers.Add (header);
               }
               remove.Add (header);
            }
            foreach (var footer in love.Where (li => li.Type == TruthType.Footer)) {
               var passage = love.FirstOrDefault (li => li.Number == footer.Number);
               if (passage != null) {
                  passage.Footers.Add (footer);
               } else {
                  love.First ().Footers.Add (footer);
               }
            }
            foreach (var levelItem in remove) {
               love.Remove (levelItem);
            }
         }
      }
   }
}