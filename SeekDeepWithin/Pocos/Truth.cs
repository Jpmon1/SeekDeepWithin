using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a truth of a light/love combination.
   /// </summary>
   public class Truth : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the type of truth.
      /// </summary>
      public int Type { get; set; }

      /// <summary>
      /// Gets or Sets the order.
      /// </summary>
      public int? Order { get; set; }

      /// <summary>
      /// Gets or Sets the number.
      /// </summary>
      public int? Number { get; set; }

      /// <summary>
      /// Gets or Sets the parent id.
      /// </summary>
      public int? ParentId { get; set; }

      /// <summary>
      /// Gets or Sets the love this truth is for.
      /// </summary>
      public virtual Love Love { get; set; }

      /// <summary>
      /// Gets or Sets the light.
      /// </summary>
      public virtual Light Light { get; set; }

      /// <summary>
      /// Gets or Sets the styles.
      /// </summary>
      public virtual ICollection<TruthStyle> Styles { get; set; }
   }
}
