using SeekDeepWithin.Domain;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a passage footer.
   /// </summary>
   public class PassageFooterViewModel : HeaderFooterViewModel
   {
      /// <summary>
      /// Initializes a new header view model.
      /// </summary>
      /// <param name="footer">Footer to copy data from.</param>
      public PassageFooterViewModel (PassageFooter footer) : base (footer) { }
   }
}