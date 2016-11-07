using System.Windows;
using System.Windows.Controls;

namespace Peter.Common.Controls.DockMan
{
   /// <summary>
   /// A docking manager that works.
   /// </summary>
   public class DockMan : Control
   {
      /// <summary>
      /// Static constructor
      /// </summary>
      static DockMan ()
      {
         DefaultStyleKeyProperty.OverrideMetadata (typeof (DockMan),
            new FrameworkPropertyMetadata (typeof (DockMan)));
      }
   }
}
