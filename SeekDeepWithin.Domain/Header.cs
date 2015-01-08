namespace SeekDeepWithin.Domain
{
   public class Header : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the header.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text of the header.
      /// </summary>
      public string Text { get; set; }
   }
}
