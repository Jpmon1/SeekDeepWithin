namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a source for a version.
   /// </summary>
   public class VersionSource : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the version source.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the version.
      /// </summary>
      public virtual Version Version { get; set; }

      /// <summary>
      /// Gets or Sets the source.
      /// </summary>
      public virtual Source Source { get; set; }
   }
}
