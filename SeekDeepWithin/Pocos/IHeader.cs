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
      /// Gets or Sets the header.
      /// </summary>
      string Text { get; set; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      IEnumerable<IStyle> StyleList { get; }
   }
}