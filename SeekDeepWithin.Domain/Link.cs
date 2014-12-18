namespace SeekDeepWithin.Domain
{
   public class Link : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the link.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the url for the link.
      /// </summary>
      public string Url { get; set; }
   }
}
