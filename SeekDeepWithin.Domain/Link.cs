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

      /// <summary>
      /// Gets or Sets if the link should open in a new window.
      /// </summary>
      public bool OpenInNewWindow { get; set; }

      /// <summary>
      /// Gets or Sets if this link exists or not.
      /// </summary>
      public bool Exists { get; set; }
   }
}
