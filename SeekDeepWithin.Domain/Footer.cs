namespace SeekDeepWithin.Domain
{
   public class Footer : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the footer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text of the footer.
      /// </summary>
      public string Text { get; set; }
   }
}
