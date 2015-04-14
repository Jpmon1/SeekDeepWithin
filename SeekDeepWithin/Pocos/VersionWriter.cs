namespace SeekDeepWithin.Pocos
{
   public class VersionWriter : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the writer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the author.
      /// </summary>
      public virtual Writer Writer { get; set; }

      /// <summary>
      /// Gets or Sets the sub book.
      /// </summary>
      public virtual Version Version { get; set; }

      public bool IsTranslator { get; set; }
   }
}