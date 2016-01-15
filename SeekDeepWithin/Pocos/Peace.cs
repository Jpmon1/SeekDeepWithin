using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class Peace : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Get or Sets the type of peace.
      /// </summary>
      public int Type { get; set; }

      /// <summary>
      /// Gets or Sets the parent light.
      /// </summary>
      public virtual Light Light { get; set; }

      /// <summary>
      /// Gets or Sets the parent loves.
      /// </summary>
      public virtual ICollection <Love> Loves { get; set; }
   }
}