namespace SeekDeepWithin.Pocos
{
   public class BookTag
   {
      /// <summary>
      /// Gets or Sets the id of the book tag.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the tag.
      /// </summary>
      public virtual Tag Tag { get; set; }

      /// <summary>
      /// Gets or Sets the book.
      /// </summary>
      public virtual Book Book { get; set; }
   }
}
