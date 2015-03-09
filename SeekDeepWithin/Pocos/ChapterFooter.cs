namespace SeekDeepWithin.Pocos
{
   public class ChapterFooter : IDbTable, IFooter
   {
      /// <summary>
      /// Gets or Sets the id of the footer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets if the content should be bolded.
      /// </summary>
      public bool IsBold { get; set; }

      /// <summary>
      /// Gets or Sets the index of the footer.
      /// </summary>
      public int Index { get; set; }

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
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the chapter the footer belongs to.
      /// </summary>
      public virtual SubBookChapter Chapter { get; set; }
   }
}
