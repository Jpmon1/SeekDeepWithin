using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Peter.Common.Icons;

namespace Peter.Common.Controls
{
   /// <summary>
   /// A pretty panel, with a title.
   /// </summary>
   public class Pane : HeaderedItemsControl
   {
      /// <summary>
      /// Static constructor
      /// </summary>
      static Pane ()
      {
         DefaultStyleKeyProperty.OverrideMetadata (typeof (Pane),
            new FrameworkPropertyMetadata (typeof (Pane)));
      }

      /// <summary>
      /// Dependency property for the footer.
      /// </summary>
      public static readonly DependencyProperty FooterProperty = DependencyProperty.Register (
         "Footer", typeof (object), typeof (Pane), new PropertyMetadata (default (object)));

      /// <summary>
      /// Gets or Sets the footer of the panel.
      /// </summary>
      public object Footer
      {
         get { return GetValue (FooterProperty); }
         set { SetValue (FooterProperty, value); }
      }

      /// <summary>
      /// Dependency property for the status.
      /// </summary>
      public static readonly DependencyProperty StatusProperty = DependencyProperty.Register (
         "Status", typeof (Status), typeof (Pane), new PropertyMetadata (default (Status)));

      /// <summary>
      /// Gets or Sets the status.
      /// </summary>
      public Status Status
      {
         get { return (Status)GetValue (StatusProperty); }
         set { SetValue (StatusProperty, value); }
      }

      /// <summary>
      /// Dependency property for collapsable.
      /// </summary>
      public static readonly DependencyProperty CanCollapseProperty = DependencyProperty.Register (
         "CanCollapse", typeof (bool), typeof (Pane), new PropertyMetadata (default (bool)));

      /// <summary>
      /// Gets or Sets if the pane can collapse.
      /// </summary>
      public bool CanCollapse
      {
         get { return (bool)GetValue (CanCollapseProperty); }
         set { SetValue (CanCollapseProperty, value); }
      }

      /// <summary>
      /// Dependency property for expanded state.
      /// </summary>
      public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register (
         "IsExpanded", typeof (bool), typeof (Pane), new FrameworkPropertyMetadata (true,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));

      /// <summary>
      /// Gets or Sets if the pane is expanded or not.
      /// </summary>
      public bool IsExpanded
      {
         get { return (bool)GetValue (IsExpandedProperty); }
         set { SetValue (IsExpandedProperty, value); }
      }

      /// <summary>
      /// When overridden in a derived class, is invoked whenever application code or internal processes call
      /// <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
      /// </summary>
      public override void OnApplyTemplate ()
      {
         base.OnApplyTemplate ();
         var toggle = this.GetTemplateChild ("PART_CollapseToggle") as IconDisplay;
         if (toggle != null)
         {
            toggle.MouseUp += this.OnToggleClicked;
         }
      }

      /// <summary>
      /// Occurs when the toggle is clicked.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void OnToggleClicked (object sender, MouseButtonEventArgs e)
      {
         this.IsExpanded = !this.IsExpanded;
      }

      /// <summary>
      /// Determines if the specified item is (or is eligible to be) its own container.
      /// </summary>
      /// <param name="item">The item to check.</param>
      /// <returns>true if the item is (or is eligible to be) its own container; otherwise, false.</returns>
      protected override bool IsItemItsOwnContainerOverride (object item)
      {
         return item is PaneItem;
      }

      /// <summary>
      /// Creates or identifies the element that is used to display the given item.
      /// </summary>
      /// <returns>The element that is used to display the given item.</returns>
      protected override DependencyObject GetContainerForItemOverride ()
      {
         return new PaneItem ();
      }
   }
}
