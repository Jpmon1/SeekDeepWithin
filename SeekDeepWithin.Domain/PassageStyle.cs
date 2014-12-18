namespace SeekDeepWithin.Domain
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
      /// Gets the passage entry this style belongs to.
      /// </summary>
      public PassageEntry PassageEntry { get; set; }

      /// <summary>
      /// Gets the Style for this...style....
      /// </summary>
      public Style Style { get; set; }
   }
}
