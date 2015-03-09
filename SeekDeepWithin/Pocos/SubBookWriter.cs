namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a writer of a version.
   /// </summary>
   public class SubBookWriter : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the writer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets if this writer is a translator.
      /// </summary>
      public bool IsTranslator { get; set; }

      /// <summary>
      /// Gets or Sets the author.
      /// </summary>
      public virtual Writer Writer { get; set; }

      /// <summary>
      /// Gets or Sets the sub book.
      /// </summary>
      public virtual SubBook SubBook { get; set; }
   }
}
