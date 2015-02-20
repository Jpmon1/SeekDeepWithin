namespace SeekDeepWithin.Domain
{
   /// <summary>
   /// Interface for footers.
   /// </summary>
   public interface IFooter
   {
      /// <summary>
      /// Gets or Sets the id of the footer.
      /// </summary>
      int Id { get; set; }

      /// <summary>
      /// Gets or Sets if the content should be bolded.
      /// </summary>
      bool IsBold { get; set; }

      /// <summary>
      /// Gets or Sets if the content should be italicized.
      /// </summary>
      bool IsItalic { get; set; }

      /// <summary>
      /// Gets or Sets the justification of the footer.
      /// </summary>
      int Justify { get; set; }

      /// <summary>
      /// Gets or Sets the footer.
      /// </summary>
      string Text { get; set; }

      /// <summary>
      /// Gets or Sets the index of the footer.
      /// </summary>
      int Index { get; set; }
   }
}