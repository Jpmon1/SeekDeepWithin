using System.Windows;
using System.Windows.Controls;

namespace Peter.Common.Controls
{
   /// <summary>
   /// A toast control
   /// </summary>
   public class Toast : ContentControl
   {
      private bool m_IsTemplateReady;

      /// <summary>
      /// Static constructor
      /// </summary>
      static Toast ()
      {
         DefaultStyleKeyProperty.OverrideMetadata (typeof (Toast),
            new FrameworkPropertyMetadata (typeof (Toast)));
      }

      /// <summary>
      /// Routed event for showing toast.
      /// </summary>
      public static readonly RoutedEvent ShowEvent = EventManager.RegisterRoutedEvent (
         "Show", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Toast));

      /// <summary>
      /// Event used to show the toast.
      /// </summary>
      public event RoutedEventHandler Show
      {
         add { AddHandler (ShowEvent, value); }
         remove { RemoveHandler (ShowEvent, value); }
      }

      /// <summary>
      /// Routed event for hiding toast.
      /// </summary>
      public static readonly RoutedEvent HideEvent = EventManager.RegisterRoutedEvent (
         "Hide", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Toast));

      /// <summary>
      /// Event used to hide the toast.
      /// </summary>
      public event RoutedEventHandler Hide
      {
         add { AddHandler (HideEvent, value); }
         remove { RemoveHandler (HideEvent, value); }
      }

      /// <summary>
      /// Dependency property for status.
      /// </summary>
      public static readonly DependencyProperty StatusProperty = DependencyProperty.Register (
         "Status", typeof (Status), typeof (Toast), new PropertyMetadata (Status.Info));

      /// <summary>
      /// Gets or Sets the status for the toast.
      /// </summary>
      public Status Status
      {
         get { return (Status) GetValue (StatusProperty); }
         set { SetValue (StatusProperty, value); }
      }

      /// <summary>
      /// Dependency property for showing the close button.
      /// </summary>
      public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register (
         "ShowCloseButton", typeof (bool), typeof (Toast), new PropertyMetadata (true));

      /// <summary>
      /// Gets or Sets if the close button should be shown or not.
      /// </summary>
      public bool ShowCloseButton
      {
         get { return (bool) GetValue (ShowCloseButtonProperty); }
         set { SetValue (ShowCloseButtonProperty, value); }
      }

      /// <summary>
      /// Dependency property for is open.
      /// </summary>
      public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register (
         "IsOpen", typeof (bool), typeof (Toast),
         new PropertyMetadata (default (bool), OnIsOpenChanged));

      /// <summary>
      /// Gets if the toast is open or not.
      /// </summary>
      public bool IsOpen
      {
         get { return (bool) GetValue (IsOpenProperty); }
         set { SetValue (IsOpenProperty, value); }
      }

      /// <summary>
      /// Occurs when the is open property changes.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnIsOpenChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var toast = d as Toast;
         if (toast != null) {
            if (toast.IsOpen)
               toast.RaiseShowEvent ();
            else
               toast.RaiseHideEvent ();
         }
      }

      /// <summary>
      /// Raises the show event.
      /// </summary>
      private void RaiseShowEvent ()
      {
         if (this.m_IsTemplateReady) {
            var eventArgs = new RoutedEventArgs (ShowEvent);
            RaiseEvent (eventArgs);
         }
      }

      /// <summary>
      /// Raises the hide event.
      /// </summary>
      private void RaiseHideEvent ()
      {
         if (this.m_IsTemplateReady) {
            var eventArgs = new RoutedEventArgs (HideEvent);
            RaiseEvent (eventArgs);
         }
      }

      /// <summary>
      /// When overridden in a derived class, is invoked whenever application code or internal processes call
      /// <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
      /// </summary>
      public override void OnApplyTemplate ()
      {
         base.OnApplyTemplate ();
         var closeButton = GetTemplateChild ("PART_CloseButton") as Button;
         if (closeButton != null) {
            closeButton.Click += this.OnClose;
         }
         this.m_IsTemplateReady = true;
         if (this.IsOpen)
            this.RaiseShowEvent ();
      }

      /// <summary>
      /// Occurs when the close button is clicked.
      /// </summary>
      /// <param name="sender">The close button.</param>
      /// <param name="e">RoutedEventArgs</param>
      private void OnClose (object sender, RoutedEventArgs e)
      {
         this.IsOpen = false;
      }
   }
}
