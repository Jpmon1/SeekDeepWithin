using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
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
      /// Gets or Sets the footer.
      /// </summary>
      string Text { get; set; }

      /// <summary>
      /// Gets or Sets the index of the footer.
      /// </summary>
      int Index { get; set; }

      /// <summary>
      /// Gets the list of links for this entry.
      /// </summary>
      IEnumerable<ILink> LinkList { get; }

      /// <summary>
      /// Gets the list of styles for this entry.
      /// </summary>
      IEnumerable<IStyle> StyleList { get; }
   }
}