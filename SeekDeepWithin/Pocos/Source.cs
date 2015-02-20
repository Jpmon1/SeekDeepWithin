namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents an internet source.
   /// </summary>
   public class Source : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the source.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of this source.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the url for the source.
      /// </summary>
      public string Url { get; set; }
   }
}
