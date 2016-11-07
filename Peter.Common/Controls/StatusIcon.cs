using System.Windows;
using System.Windows.Controls;

namespace Peter.Common.Controls
{
   /// <summary>
   /// A display for a status icon.
   /// </summary>
   public class StatusIcon : Control
   {
      /// <summary>
      /// Static constructor
      /// </summary>
      static StatusIcon ()
      {
         DefaultStyleKeyProperty.OverrideMetadata (typeof (StatusIcon),
            new FrameworkPropertyMetadata (typeof (StatusIcon)));
      }

      /// <summary>
      /// Dependency property for the status.
      /// </summary>
      public static readonly DependencyProperty StatusProperty = DependencyProperty.Register (
         "Status", typeof (Status), typeof (StatusIcon),
         new PropertyMetadata (default (Status)));

      /// <summary>
      /// Gets or Sets the status.
      /// </summary>
      public Status Status
      {
         get { return (Status) GetValue (StatusProperty); }
         set { SetValue (StatusProperty, value); }
      }
   }
}
