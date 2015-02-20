namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a link for a given passage.
   /// </summary>
   public class PassageLink
   {
      /// <summary>
      /// Gets or Sets the id of the passage link.
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
      /// Gets or Sets the passage.
      /// </summary>
      public virtual Passage Passage { get; set; }

      /// <summary>
      /// Gets or Sets the link.
      /// </summary>
      public virtual Link Link { get; set; }
   }
}
