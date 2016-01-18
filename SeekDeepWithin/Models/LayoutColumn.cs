namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a column in a level.
   /// </summary>
   public class LayoutColumn
   {
      /// <summary>
      /// Gets or Sets the small span.
      /// </summary>
      public int SmallSpan { get; set; }

      /// <summary>
      /// Gets or Sets the medium span.
      /// </summary>
      public int MediumSpan { get; set; }

      /// <summary>
      /// Gets or Sets the large span.
      /// </summary>
      public int LargeSpan { get; set; }

      /// <summary>
      /// Gets or Sets the Item to display.
      /// </summary>
      public SdwItem SdwItem { get; set; }
   }
}