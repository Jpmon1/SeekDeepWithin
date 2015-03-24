namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a tag for a sub book.
   /// </summary>
   public class SubBookTag
   {
      /// <summary>
      /// Gets or Sets the id of the glossary term tag.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the tag.
      /// </summary>
      public virtual Tag Tag { get; set; }

      /// <summary>
      /// Gets or Sets the sub book.
      /// </summary>
      public virtual SubBook SubBook { get; set; }
   }
}