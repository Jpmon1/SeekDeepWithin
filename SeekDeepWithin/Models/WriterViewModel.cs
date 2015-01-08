namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents an author link.
   /// </summary>
   public class WriterViewModel
   {
      /// <summary>
      /// Gets or Sets the id of the author.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of the author.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets if the author of the version is a translator.
      /// </summary>
      public bool IsTranslator { get; set; }
   }
}