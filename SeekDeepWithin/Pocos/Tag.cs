namespace SeekDeepWithin.Pocos
{
   public class Tag : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the tag.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Get or Sets name of this tag.
      /// </summary>
      public string Name { get; set; }
   }
}
