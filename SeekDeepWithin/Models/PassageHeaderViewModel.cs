using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a passage header.
   /// </summary>
   public class PassageHeaderViewModel : HeaderFooterViewModel
   {
      /// <summary>
      /// Initializes a new header view model.
      /// </summary>
      /// <param name="header">Header to copy data from.</param>
      public PassageHeaderViewModel (PassageHeader header) : base (header) { }
   }
}