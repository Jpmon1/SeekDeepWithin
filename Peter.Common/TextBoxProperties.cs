using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Peter.Common.Controls;

namespace Peter.Common
{
   /// <summary>
   /// Attached properties for a text box.
   /// </summary>
   public static class TextBoxProperties
   {
      /// <summary>
      /// Attached property that will turn a text box into numeric input only.
      /// </summary>
      public static readonly DependencyProperty IsNumericProperty = DependencyProperty.RegisterAttached (
         "IsNumeric", typeof (bool), typeof (TextBoxProperties),
         new PropertyMetadata (default (bool), IsNumericChanged));

      /// <summary>
      /// Sets the is numeric property.
      /// </summary>
      /// <param name="element">Element to set property for.</param>
      /// <param name="value">The value to set.</param>
      public static void SetIsNumeric (DependencyObject element, bool value)
      {
         element.SetValue (IsNumericProperty, value);
      }

      /// <summary>
      /// Gets if the element is numeric.
      /// </summary>
      /// <param name="element">Element to get property for.</param>
      /// <returns>True if the element has the is numeric property set.</returns>
      public static bool GetIsNumeric (DependencyObject element)
      {
         return (bool) element.GetValue (IsNumericProperty);
      }

      /// <summary>
      /// Attached property that will adjust the width of the text box based on how many characters should be displayed.
      /// </summary>
      public static readonly DependencyProperty CharacterWidthProperty = DependencyProperty.RegisterAttached (
         "CharacterWidth", typeof (int), typeof (TextBoxProperties),
         new PropertyMetadata (7, OnCharacterWidthChanged));

      /// <summary>
      /// Sets the character width property.
      /// </summary>
      /// <param name="element">Element to set property for.</param>
      /// <param name="value">The value to set.</param>
      public static void SetCharacterWidth (DependencyObject element, int value)
      {
         element.SetValue (CharacterWidthProperty, value);
      }

      /// <summary>
      /// Gets if the character width of the element.
      /// </summary>
      /// <param name="element">Element to get property for.</param>
      /// <returns>True if the element has the is numeric property set.</returns>
      public static int GetCharacterWidth (DependencyObject element)
      {
         return (int) element.GetValue (CharacterWidthProperty);
      }

      /// <summary>
      /// Occurs when the is numeric property changes.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void IsNumericChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var textBox = (TextBox) d;
         if (textBox != null) {
            if (e.OldValue != null) {
               textBox.PreviewTextInput -= OnPreviewInputText;
               textBox.PreviewKeyDown -= OnPreviewKeyDown;

            }
            if ((bool) e.NewValue) {
               textBox.PreviewTextInput += OnPreviewInputText;
               textBox.PreviewKeyDown += OnPreviewKeyDown;
            }
         }
      }

      /// <summary>
      /// Occurs when the character width property changes.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnCharacterWidthChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var textBox = (Control) d;
         if (textBox != null) {
            if (e.OldValue != null)
               textBox.Width = double.NaN;

            if (e.NewValue != null)
               UpdateTextBoxWidth (textBox, (int) e.NewValue);
         }
      }

      /// <summary>
      /// Updates the width of the text box.
      /// </summary>
      private static void UpdateTextBoxWidth (Control textBox, int width)
      {
         var text = string.Empty;
         for (int i = 0; i <= (width + 1); i++) {
            text += "0";
         }
         var textBlock = new TextBlock {
            Text = text,
            TextWrapping = TextWrapping.Wrap,
            FontFamily = textBox.FontFamily,
            FontStyle = textBox.FontStyle,
            FontWeight = textBox.FontWeight,
            FontStretch = textBox.FontStretch,
            FontSize = textBox.FontSize
         };
         textBlock.Measure (new Size (double.PositiveInfinity, double.PositiveInfinity));
         textBlock.Arrange (new Rect (textBlock.DesiredSize));
         textBox.Width = textBlock.ActualWidth;
      }

      /// <summary>
      /// Verifies the input.
      /// </summary>
      /// <param name="sender">Input TextBox.</param>
      /// <param name="e">TextCompositionEventArgs</param>
      private static void OnPreviewInputText (object sender, TextCompositionEventArgs e)
      {
         var textBox = (TextBox) sender;
         // If text box contains selected text we should replace it with e.Text
         string fullText = textBox.SelectionLength > 0
           ? textBox.Text.Replace (textBox.SelectedText, e.Text)
           : textBox.Text.Insert (textBox.CaretIndex, e.Text);
         e.Handled = !DoubleValidator.IsValid (fullText);
      }

      /// <summary>
      /// Occurs when a key is pressed in the input box.
      /// Checks to see if the key is a space, if so ignore it for double input.
      /// </summary>
      /// <param name="sender">Input TextBox</param>
      /// <param name="e">KeyEventArgs</param>
      private static void OnPreviewKeyDown (object sender, KeyEventArgs e)
      {
         var textBox = (TextBox) sender;
         if (e.Key == Key.Space ||
             e.Key == Key.OemOpenBrackets ||
             e.Key == Key.OemCloseBrackets ||
             e.Key == Key.Multiply ||
             e.Key == Key.Divide ||
             e.Key == Key.OemBackslash ||
             e.Key == Key.OemPipe ||
             e.Key == Key.OemSemicolon ||
             e.Key == Key.OemTilde ||
             e.Key == Key.OemQuestion ||
             e.Key == Key.OemQuotes ||
             e.Key == Key.OemComma ||
            ((e.Key == Key.OemPeriod || e.Key == Key.Decimal) && textBox.Text.Contains ("."))) {
            e.Handled = true;
         }
      }
   }
}
