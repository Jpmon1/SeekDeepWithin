namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Common interface for styles.
   /// </summary>
   public interface IStyle
   {
      /// <summary>
      /// Gets or Sets the id of the entry style.
      /// </summary>
      int Id { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the style.
      /// </summary>
      int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the style.
      /// </summary>
      int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets the style.
      /// </summary>
      Style Style { get; set; }
   }
}