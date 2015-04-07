namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a style for a glossary entry.
   /// </summary>
   public class GlossaryEntryStyle : IDbTable, IStyle
   {
      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the style.
      /// </summary>
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the style.
      /// </summary>
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets the glossary entry this style belongs to.
      /// </summary>
      public virtual GlossaryEntry Entry { get; set; }

      /// <summary>
      /// Gets the Style for this...style....
      /// </summary>
      public virtual Style Style { get; set; }
   }
}