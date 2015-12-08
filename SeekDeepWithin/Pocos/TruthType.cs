namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Gets the type of love.
   /// </summary>
   public enum TruthType
   {
      /// <summary>
      /// A category/tag .
      /// </summary>
      Tag = 1,

      Book,

      Section,

      Passage,

      PublishDate,

      Summary,

      SourceUrl,

      SourceName,

      SubTitle,

      Term,

      Reference,

      AlsoKnownAs,

      /// <summary>
      /// Someone who authored a book.
      /// </summary>
      Author,

      /// <summary>
      /// Someone who translated a book.
      /// </summary>
      Translator,

      /// <summary>
      /// A related light.
      /// </summary>
      SeeAlso,

      /// <summary>
      /// A link.
      /// </summary>
      Link,

      /// <summary>
      /// A header.
      /// </summary>
      Header,

      /// <summary>
      /// A footer.
      /// </summary>
      Footer
   }
}