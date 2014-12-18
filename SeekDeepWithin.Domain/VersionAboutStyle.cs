namespace SeekDeepWithin.Domain
{
   /// <summary>
   /// Represents a style found in a version's about blurb.
   /// </summary>
   public class VersionAboutStyle : IDbTable
   {
      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets the version this style belongs to.
      /// </summary>
      public Version Vesion { get; set; }

      /// <summary>
      /// Gets the Style for this...style....
      /// </summary>
      public Style Style { get; set; }
   }
}
