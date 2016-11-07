using System.Windows;
using System.Windows.Controls;

namespace Peter.Common.Controls
{
   /// <summary>
   /// A item to insert into a pane.
   /// </summary>
   public class PaneItem : ContentControl
   {
      /// <summary>
      /// Static constructor
      /// </summary>
      static PaneItem ()
      {
         DefaultStyleKeyProperty.OverrideMetadata (typeof (PaneItem),
            new FrameworkPropertyMetadata (typeof (PaneItem)));
      }
   }
}
