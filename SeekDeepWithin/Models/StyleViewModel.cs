namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a style
   /// </summary>
   public class StyleViewModel
   {
      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int ItemId { get; set; }

      /// <summary>
      /// Gets or Sets the start of the style.
      /// </summary>
      public string Start { get; set; }

      /// <summary>
      /// Gets or Sets if this style spans multiple items (entries/passages).
      /// </summary>
      public bool SpansMultiple { get; set; }

      /// <summary>
      /// Gets or Sets the end of the style.
      /// </summary>
      public string End { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the style.
      /// </summary>
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the style.
      /// </summary>
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets the id of the style
      /// </summary>
      public int StyleId { get; set; }
   }
}