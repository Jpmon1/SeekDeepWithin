namespace SeekDeepWithin.Domain
{
   public class PassageFooter
   {
      /// <summary>
      /// Gets or Sets the id of the footer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text of the footer.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the passage the footer belongs to.
      /// </summary>
      public virtual PassageEntry Passage { get; set; }
   }
}
