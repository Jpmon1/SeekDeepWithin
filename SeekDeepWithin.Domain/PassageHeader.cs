namespace SeekDeepWithin.Domain
{
   public class PassageHeader
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
      /// Gets or Sets the passage the header belongs to.
      /// </summary>
      public virtual PassageEntry Passage { get; set; }
   }
}
