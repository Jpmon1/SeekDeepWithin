namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a style found in a passage.
   /// </summary>
   public class PassageStyle : IDbTable
   {
      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the style.
      /// </summary>
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the style.
      /// </summary>
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets the passage entry this style belongs to.
      /// </summary>
      public virtual PassageEntry PassageEntry { get; set; }

      /// <summary>
      /// Gets the Style for this...style....
      /// </summary>
      public virtual Style Style { get; set; }
   }
}
