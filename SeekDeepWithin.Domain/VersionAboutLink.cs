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
      /// Gets or Sets the start index of the link.
      /// </summary>
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the link.
      /// </summary>
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets if the link should open in a new window.
      /// </summary>
      public bool OpenInNewWindow { get; set; }

      /// <summary>
      /// Gets the version this link belongs to.
      /// </summary>
      public virtual Version Vesion { get; set; }

      /// <summary>
      /// Gets the Link for this...link....
      /// </summary>
      public virtual Link Link { get; set; }
   }
}
