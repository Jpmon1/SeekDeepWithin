using System;
using SeekDeepWithin.Controllers;

namespace SeekDeepWithin.Models
{
   public class SoulItem
   {
      public SoulItem (string item, Hashids hash)
      {
         if (item.StartsWith ("S,")) {
            this.IsSelected = true;
            item = item.Substring (2);
         }
         var itemSplit = item.Split (',');
         if (itemSplit [0] == "L")
            this.IsLight = true;
         this.Id = Convert.ToInt32 (itemSplit [1]);
         this.Key = itemSplit [2];
         if (!this.IsLight)
            this.ParentLights = hash.Decode (this.Key);
      }

      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; private set; }

      /// <summary>
      /// Gets the key of the item.
      /// </summary>
      public string Key { get; private set; }

      /// <summary>
      /// Gets if this item is a light.
      /// </summary>
      public bool IsLight { get; private set; }

      /// <summary>
      /// Gets or Sets if this item is selected.
      /// </summary>
      public bool IsSelected { get; private set; }

      /// <summary>
      /// Gets or Sets any parent lights.
      /// </summary>
      public int[] ParentLights { get; set; }
   }
}