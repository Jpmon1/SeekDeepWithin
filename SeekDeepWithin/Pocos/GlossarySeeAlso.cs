namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a see also item for a glossary term.
   /// </summary>
   public class GlossarySeeAlso : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of the see also link.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the term.
      /// </summary>
      public virtual GlossaryTerm Term { get; set; }

      /// <summary>
      /// Gets or Sets the Link.
      /// </summary>
      public virtual Link Link { get; set; }
   }
}