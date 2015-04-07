using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Interface for headers.
   /// </summary>
   public interface IHeader
   {
      /// <summary>
      /// Gets or Sets the id of the header.
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
      /// Gets or Sets the justification of the header.
      /// </summary>
      int Justify { get; set; }

      /// <summary>
      /// Gets or Sets the header.
      /// </summary>
      string Text { get; set; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      IEnumerable<IStyle> StyleList { get; }
   }
}