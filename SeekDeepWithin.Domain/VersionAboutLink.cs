namespace SeekDeepWithin.Domain
{
   /// <summary>
   /// Represents a link found in a version's about blurb.
   /// </summary>
   public class VersionAboutLink : IDbTable
   {
      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets the version this link belongs to.
      /// </summary>
      public Version Vesion { get; set; }

      /// <summary>
      /// Gets the Link for this...link....
      /// </summary>
      public Link Link { get; set; }
   }
}
