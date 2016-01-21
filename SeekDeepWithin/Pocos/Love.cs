using System;
using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a link of love.
   /// </summary>
   public class Love : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the last time this was modified.
      /// </summary>
      public DateTime Modified { get; set; }

      /// <summary>
      /// Gets or Sets the id of the peace.
      /// </summary>
      public string PeaceId { get; set; }

      /// <summary>
      /// Gets or Sets the parent peace.
      /// </summary>
      public virtual ICollection<Peace> Peaces { get; set; }

      /// <summary>
      /// Gets or Sets the truths.
      /// </summary>
      public virtual ICollection<Truth> Truths { get; set; }
   }
}