namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a link for a glossary entry.
   /// </summary>
   public class GlossaryEntryLink
   {
      /// <summary>
      /// Gets or Sets the id of the entry link.
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
      public bool OpenInNewWindow { get; set; }

      /// <summary>
      /// Gets or Sets the glossary entry.
      /// </summary>
      public virtual GlossaryEntry Entry { get; set; }

      /// <summary>
      /// Gets or Sets the link.
      /// </summary>
      public virtual Link Link { get; set; }
   }
}