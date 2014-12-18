namespace SeekDeepWithin.Domain
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
   }
}
