namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents an amazon item for a glossary term.
   /// </summary>
   public class GlossaryAmazonItem : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the term.
      /// </summary>
      public GlossaryTerm Term { get; set; }

      /// <summary>
      /// Gets or Sets the amazon item.
      /// </summary>
      public AmazonItem AmazonItem { get; set; }
   }
}