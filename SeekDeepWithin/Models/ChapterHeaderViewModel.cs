using SeekDeepWithin.Domain;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a chapter header.
   /// </summary>
   public class ChapterHeaderViewModel : HeaderFooterViewModel
   {
      /// <summary>
      /// Initializes a new header view model.
      /// </summary>
      /// <param name="header">Header to copy data from.</param>
      public ChapterHeaderViewModel (ChapterHeader header) : base (header) {}
   }
}