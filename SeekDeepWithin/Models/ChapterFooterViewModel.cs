using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a chapter footer.
   /// </summary>
   public class ChapterFooterViewModel : HeaderFooterViewModel
   {
      /// <summary>
      /// Initializes a new header view model.
      /// </summary>
      /// <param name="footer">Footer to copy data from.</param>
      public ChapterFooterViewModel (ChapterFooter footer) : base (footer) { }
   }
}