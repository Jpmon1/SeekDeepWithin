namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a writer of a version.
   /// </summary>
   public class TermWriter : IDbTable
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
      /// Gets or Sets the writer.
      /// </summary>
      public virtual Term Writer { get; set; }

      /// <summary>
      /// Gets or Sets the item the writer wrote.
      /// </summary>
      public virtual Term Wrote { get; set; }
   }
}
