namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents an author link.
   /// </summary>
   public class WriterLink
   {
      /// <summary>
      /// Gets or Sets the id of the writer.
      /// </summary>
      public int AuthorId { get; set; }

      /// <summary>
      /// Gets or Sets the author.
      /// </summary>
      public AuthorViewModel Author { get; set; }

      /// <summary>
      /// Gets or Sets the version.
      /// </summary>
      public VersionViewModel Version { get; set; }

      /// <summary>
      /// Gets or Sets if the author of the version is a translator.
      /// </summary>
      public bool IsTranslator { get; set; }
   }
}