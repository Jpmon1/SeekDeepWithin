using System;
using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// A basic piece of light
   /// </summary>
   public class Light : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text of this light.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the last time this was modified.
      /// </summary>
      public DateTime Modified { get; set; }

      /// <summary>
      /// Gets the collection of truth for this light.
      /// </summary>
      public virtual ICollection<Truth> Truths { get; set; }

      /// <summary>
      /// Gets the collection of love for this light.
      /// </summary>
      public virtual ICollection<Love> Loves { get; set; }
   }
}
