namespace SeekDeepWithin.Pocos
{
   public class VersionWriter : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the writer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets if this is a translator.
      /// </summary>
      public bool IsTranslator { get; set; }

      /// <summary>
      /// Gets or Sets the writer.
      /// </summary>
      public virtual Term Writer { get; set; }

      /// <summary>
      /// Gets or Sets the sub book.
      /// </summary>
      public virtual Version Version { get; set; }
   }
}