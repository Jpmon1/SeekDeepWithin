namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a amazon item.
   /// </summary>
   public class AmazonItem : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the amazon id of the item.
      /// </summary>
      public string AmazonId { get; set; }
   }
}