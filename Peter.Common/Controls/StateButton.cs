using System.Windows;
using System.Windows.Controls;
using Peter.Common.MainMenu;

namespace Peter.Common.Controls
{
   /// <summary>
   /// A button that can have different states.
   /// </summary>
   public class StateButton : Button
   {
      /// <summary>
      /// Static constructor
      /// </summary>
      static StateButton ()
      {
         DefaultStyleKeyProperty.OverrideMetadata (typeof(StateButton),
            new FrameworkPropertyMetadata (typeof(StateButton)));
      }

      /// <summary>
      /// Dependency property for the glyph.
      /// </summary>
      public static readonly DependencyProperty IconProperty = DependencyProperty.Register (
         "Icon", typeof (MainMenuIcon), typeof (StateButton),
         new PropertyMetadata (MainMenuIcon.None));

      /// <summary>
      /// Gets or Sets the glyph for the button.
      /// </summary>
      public MainMenuIcon Icon
      {
         get { return (MainMenuIcon) GetValue (IconProperty); }
         set { SetValue (IconProperty, value); }
      }

      /// <summary>
      /// Dependency property for the current state.
      /// </summary>
      public static readonly DependencyProperty StateProperty = DependencyProperty.Register (
         "State", typeof(State), typeof(StateButton), new PropertyMetadata (default(State)));

      /// <summary>
      /// Gets or Sets the current state.
      /// </summary>
      public State State
      {
         get { return (State) GetValue (StateProperty); }
         set { SetValue (StateProperty, value); }
      }

      /// <summary>
      /// Dependency property for the corner radius.
      /// </summary>
      public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register (
         "CornerRadius", typeof(CornerRadius), typeof(StateButton),
         new PropertyMetadata (new CornerRadius (5)));

      /// <summary>
      /// Gets or Sets the corner radius for the button.
      /// </summary>
      public CornerRadius CornerRadius
      {
         get { return (CornerRadius) GetValue (CornerRadiusProperty); }
         set { SetValue (CornerRadiusProperty, value); }
      }

      /// <summary>
      /// Dependency property for status.
      /// </summary>
      public static readonly DependencyProperty StatusProperty = DependencyProperty.Register (
         "Status", typeof(Status), typeof(StateButton), new PropertyMetadata (default(Status)));

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
