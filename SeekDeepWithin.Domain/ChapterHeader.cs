namespace SeekDeepWithin.Domain
{
   public class ChapterHeader
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
      public virtual Header Header { get; set; }

      /// <summary>
      /// Gets or Sets the chapter the header belongs to.
      /// </summary>
      public virtual Chapter Chapter { get; set; }
   }
}
