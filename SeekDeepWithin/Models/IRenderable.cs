using System.Collections.ObjectModel;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a renderable item.
   /// </summary>
   public interface IRenderable
   {
      /// <summary>
      /// Gets or Sets the main text of this renderable.
      /// </summary>
      string Text { get; }

      /// <summary>
      /// Gets the list of links for this renderable.
      /// </summary>
      Collection <LinkViewModel> Links { get; }

      /// <summary>
      /// Get or Sets the styles for this renderable.
      /// </summary>
      Collection <StyleViewModel> Styles { get; }

      /// <summary>
      /// Get or Sets the footers for this passage.
      /// </summary>
      Collection <HeaderFooterViewModel> Footers { get; }
   }
}