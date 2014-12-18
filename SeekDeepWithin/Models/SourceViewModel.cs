namespace SeekDeepWithin.Models
{
   public class SourceViewModel
   {
      /// <summary>
      /// Gets or Sets the id of the source.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the type of source (entry, version, etc...)
      /// </summary>
      public string Type { get; set; }

      /// <summary>
      /// Gets or Sets the name of this source.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the url for the source.
      /// </summary>
      public string Url { get; set; }
   }
}