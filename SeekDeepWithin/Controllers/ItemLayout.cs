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
   public class ItemLayout
   {
      private readonly ItemModel m_Item;

      /// <summary>
      /// Initializes a new level layout.
      /// </summary>
      /// <param name="item">The level to layout.</param>
      public ItemLayout (ItemModel item)
      {
         this.Rows = new List <LayoutRow> ();
         this.m_Item = item;
         this.Layout ();
      }

      /// <summary>
      /// Gets the list of rows.
      /// </summary>
      public List<LayoutRow> Rows { get; private set; }

      /// <summary>
      /// Lays out the given level.
      /// </summary>
      private void Layout ()
      {
         this.Rows.Clear ();
         var loveIds = new List <int> ();
         foreach (var li in this.m_Item.ToAdd) {
            if (!loveIds.Contains (li.LoveId))
               loveIds.Add (li.LoveId);
         }
         var loves = loveIds.OrderBy(i => i).Select (id => this.m_Item.ToAdd.OrderBy (i => i.Order)
            .Where (i => i.LoveId == id).ToList ()).ToList ();
         var span = 3;
         var loveCount = loveIds.Count;
         if (loveCount > 1) {
            span = 12 / loveIds.Count;
         }
         if (span < 3) span = 3;
         AddColumns (loves, 0, 4, loveCount);
         var truthTypes = Enum.GetValues (typeof (SdwType)).Cast<SdwType> ();
         foreach (var type in truthTypes) {
            AddColumns (loves, type, span, loveCount);
         }
         for (var i = this.Rows.Count - 1; i >= 0; i--) {
            if (this.Rows [i].Columns.Count <= 0)
               this.Rows.RemoveAt (i);
            else
               this.Rows [i].MakeReady ();
         }
      }

      /// <summary>
      /// Adds columns for the given truth type.
      /// </summary>
      /// <param name="loves">Loves to look in.</param>
      /// <param name="sdwType">Truth type to find.</param>
      /// <param name="span">The suggested span.</param>
      /// <param name="loveCount">The number of loves.</param>
      private void AddColumns (IEnumerable <List <SdwItem>> loves, SdwType sdwType, int span, int loveCount)
      {
         var items = loves.Select (love => love.Where (li => li.Type == sdwType).ToList ()).ToList ();
         if (items.First ().Count > 0) {
            var row = new LayoutRow ();
            this.Rows.Add (row);
            var max = items.Max (i => i.Count);
            for (int i = 0; i < max; i++) {
               foreach (var item in items) {
                  var cSpan = span;
                  SdwItem li = null;
                  if (item.Count > i) {
                     li = item [i];
                     if (loveCount == 1 && (sdwType == SdwType.Summary || sdwType == SdwType.Passage)) {
                        cSpan = 12;
                     }
                     if (row.Span + cSpan > 12) {
                        row = new LayoutRow ();
                        this.Rows.Add (row);
                     }
                  }
                  row.AddColumn (cSpan, li);
               }
            }
         }
      }
   }
}