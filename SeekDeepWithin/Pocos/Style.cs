using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a style.
   /// </summary>
   public class Style : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the start of the style.
      /// </summary>
      public string Start { get; set; }

      /// <summary>
      /// Gets or Sets the end of the style.
      /// </summary>
      public string End { get; set; }

      /// <summary>
      /// Gets the collection of truths that have this style.
      /// </summary>
      public virtual ICollection<TruthStyle> Truths { get; set; }
   }
}
