namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents the location of a passage.
   /// </summary>
   public class PassageLocation
   {
      /// <summary>
      /// Gets or Sets the book.
      /// </summary>
      public string Book { get; set; }

      /// <summary>
      /// Gets or Sets the version.
      /// </summary>
      public string Version { get; set; }

      /// <summary>
      /// Gets or Sets the sub book.
      /// </summary>
      public string SubBook { get; set; }

      /// <summary>
      /// Gets or Sets the chapter.
      /// </summary>
      public string Chapter { get; set; }

      /// <summary>
      /// Gets or Sets the passage.
      /// </summary>
      public string Passage { get; set; }
   }
}