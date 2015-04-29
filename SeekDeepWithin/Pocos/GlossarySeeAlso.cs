namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a see also item for a glossary term.
   /// </summary>
   public class GlossarySeeAlso : IDbTable, ILink
   {
      /// <summary>
      /// Gets or Sets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the link.
      /// </summary>
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the link.
      /// </summary>
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets if the link should open in a new window.
      /// </summary>
      public bool OpenInNewWindow { get { return false; } set{} }

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