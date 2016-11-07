/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 *
 *  This code is provided on an AS IS basis, with no WARRANTIES,
 *  CONDITIONS or GUARANTEES of any kind.
 *
 **/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peter.Common.Controls
{
   /// <summary>
   /// Text box with a command and placeholder text.
   /// </summary>
   public class CommandTextBox : TextBox
   {
      /// <summary>
      /// Initializes the search text box.
      /// </summary>
      public CommandTextBox ()
      {
         this.Loaded += this.OnLoad;
      }

      /// <summary>
      /// Occurs when we are loaded.
      /// </summary>
      /// <param name="sender">this.</param>
      /// <param name="e">RoutedEventArgs</param>
      private void OnLoad (object sender, RoutedEventArgs e)
      {
         this.Loaded -= this.OnLoad;
         this.PreviewKeyDown += this.KeyPressed;
      }

      /// <summary>
      /// DependencyProperty for the caret position.
      /// </summary>
      public static readonly DependencyProperty CaretPositionProperty =
         DependencyProperty.Register ("CaretPosition", typeof (int), typeof (CommandTextBox),
         new PropertyMetadata (0, OnCaretPositionChanged, CoerceCaretPosition));

      /// <summary>
      /// Gets or Sets the caret position.
      /// </summary>
      public int CaretPosition
      {
         get { return (int)GetValue (CaretPositionProperty); }
         set { SetValue (CaretPositionProperty, value); }
      }

      /// <summary>
      /// Coerces the position to the correct values.
      /// </summary>
      /// <param name="d">Combo box.</param>
      /// <param name="value">Current value.</param>
      /// <returns>Correct value.</returns>
      private static object CoerceCaretPosition (DependencyObject d, object value)
      {
         var commandTextBox = d as CommandTextBox;
         if (commandTextBox == null) return value;

         var correct = (int)value;
         if (correct > commandTextBox.Text.Length)
            correct = commandTextBox.Text.Length;
         else if (correct < 0)
            correct = 0;
         return correct;
      }

      /// <summary>
      /// Sets the position of the carret.
      /// </summary>
      private void SetCaret ()
      {
         this.SelectionStart = this.CaretPosition;
         this.SelectionLength = 0;
      }

      private static void OnCaretPositionChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var combo = d as CommandTextBox;
         if (combo != null)
            combo.SetCaret ();
      }

      /// <summary>
      /// Dependency Property for search command.
      /// </summary>
      public static DependencyProperty CommandProperty =
          DependencyProperty.Register (
              "Command",
              typeof (ICommand),
              typeof (CommandTextBox));

      /// <summary>
      /// Gets or Sets the command used for searching.
      /// </summary>
      public ICommand Command
      {
         get { return (ICommand)GetValue (CommandProperty); }
         set { SetValue (CommandProperty, value); }
      }

      /// <summary>
      /// Dependency Property for search command parameter.
      /// </summary>
      public static DependencyProperty CommandParameterProperty =
          DependencyProperty.Register (
              "CommandParameter",
              typeof (object),
              typeof (CommandTextBox));

      /// <summary>
      /// Gets or Sets the command parameter used for searching.
      /// </summary>
      public object CommandParameter
      {
         get { return GetValue (CommandParameterProperty); }
         set { SetValue (CommandParameterProperty, value); }
      }

      /// <summary>
      /// Dependency property for the key press command.
      /// </summary>
      public static readonly DependencyProperty KeyPressCommandProperty = DependencyProperty.Register (
         "KeyPressCommand", typeof (ICommand), typeof (CommandTextBox), new PropertyMetadata (default(ICommand)));

      /// <summary>
      /// Gets or Sets the command for a key press.
      /// </summary>
      public ICommand KeyPressCommand
      {
         get { return (ICommand) GetValue (KeyPressCommandProperty); }
         set { SetValue (KeyPressCommandProperty, value); }
      }

      /// <summary>
      /// Dependency Property for text to display when no text is entered.
      /// </summary>
      public static DependencyProperty PlaceHolderTextProperty =
          DependencyProperty.Register ("PlaceHolderText", typeof (string), typeof (CommandTextBox),
          new PropertyMetadata ("Find...", OnPlaceHolderTextChanged));

      /// <summary>
      /// Occurs when the place holder text changes.
      /// </summary>
      /// <param name="d">CommandTextBox that changed.</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnPlaceHolderTextChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var search = d as CommandTextBox;
         if (search != null && e.NewValue != null)
         {
            search.SetWatermark (e.NewValue.ToString ());
         }
      }

      /// <summary>
      /// Gets or Sets the place holder text.
      /// </summary>
      public string PlaceHolderText
      {
         get { return (string)GetValue (PlaceHolderTextProperty); }
         set { SetValue (PlaceHolderTextProperty, value); }
      }

      /// <summary>
      /// Occurs when a key is pressed in the text box.
      /// </summary>
      /// <param name="sender">This text box.</param>
      /// <param name="e">KeyEventArgs</param>
      private void KeyPressed (object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter && !e.Handled)
         {
            e.Handled = true;
            this.ExecuteCommand ();
         }
         if (this.KeyPressCommand != null)
         {
            if (this.KeyPressCommand.CanExecute (e))
               this.KeyPressCommand.Execute (e);
         }
      }

      /// <summary>
      /// Executes the attached command.
      /// </summary>
      private void ExecuteCommand ()
      {
         var command = this.Command;
         if (command != null && command.CanExecute (this.CommandParameter))
            command.Execute (this.CommandParameter);
      }

      /// <summary>
      /// Sets the water mark on the text box.
      /// </summary>
      /// <param name="text">Text to set water mark to.</param>
      private void SetWatermark (string text)
      {
         WatermarkService.SetWatermark (this, new TextBlock
                                                 {
                                                    Text = text,
                                                    Margin = new Thickness (3),
                                                    FontStyle = FontStyles.Italic
                                                 });
      }
   }
}
