namespace SeekDeepWithin.Domain
{
   public class ChapterHeader
   {
      /// <summary>
      /// Gets or Sets the id of the header.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text of the header.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the chapter the header belongs to.
      /// </summary>
      public virtual Chapter Passage { get; set; }
   }
}
