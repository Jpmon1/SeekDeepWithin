namespace SeekDeepWithin.Domain
{
   public class PassageHeader : IDbTable, IHeader
   {
      /// <summary>
      /// Gets or Sets the id of the header.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets if the content should be bolded.
      /// </summary>
      public bool IsBold { get; set; }

      /// <summary>
      /// Gets or Sets if the content should be italicized.
      /// </summary>
      public bool IsItalic { get; set; }

      /// <summary>
      /// Gets or Sets the justification of the header.
      /// </summary>
      public int Justify { get; set; }

      /// <summary>
      /// Gets or Sets the header.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the passage the header belongs to.
      /// </summary>
      public virtual PassageEntry Passage { get; set; }
   }
}
