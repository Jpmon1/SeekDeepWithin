namespace SeekDeepWithin.Domain
{
   public class PassageFooter
   {
      /// <summary>
      /// Gets or Sets the id of the footer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the index of the footer.
      /// </summary>
      public int Index { get; set; }

      /// <summary>
      /// Gets or Sets if the content should be bolded.
      /// </summary>
      public bool IsBold { get; set; }

      /// <summary>
      /// Gets or Sets if the content should be italicized.
      /// </summary>
      public bool IsItalic { get; set; }

      /// <summary>
      /// Gets or Sets the justification of the footer.
      /// </summary>
      public int Justify { get; set; }

      /// <summary>
      /// Gets or Sets the footer.
      /// </summary>
      public virtual Footer Footer { get; set; }

      /// <summary>
      /// Gets or Sets the passage the footer belongs to.
      /// </summary>
      public virtual PassageEntry Passage { get; set; }
   }
}
