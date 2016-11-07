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

namespace Peter.Common.Controls
{
   /// <summary>
   /// An extended combo box.
   /// </summary>
   public class ComboBoxEx : ComboBox
   {
      private TextBox m_TextBox;

      /// <summary>
      /// DependencyProperty for the caret position.
      /// </summary>
      public static readonly DependencyProperty CaretPositionProperty =
         DependencyProperty.Register ("CaretPosition", typeof (int), typeof (ComboBoxEx),
         new PropertyMetadata (0, OnCaretPositionChanged, CoerceCaretPosition));

      /// <summary>
      /// Gets or Sets the caret position.
      /// </summary>
      public int CaretPosition
      {
         get { return (int) GetValue (CaretPositionProperty); }
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
         var combo = (ComboBoxEx)d;
         if (combo.m_TextBox == null) return value;

         var correct = (int)value;
         if (correct > combo.m_TextBox.Text.Length)
            correct = combo.m_TextBox.Text.Length;
         else if (correct < 0)
            correct = 0;
         return correct;
      }

      /// <summary>
      /// Sets the position of the carret.
      /// </summary>
      private void SetCaret ()
      {
         this.m_TextBox.SelectionStart = this.CaretPosition;
         this.m_TextBox.SelectionLength = 0;
      }

      private static void OnCaretPositionChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var combo = d as ComboBoxEx;
         if (combo != null)
            combo.SetCaret ();
      }

      /// <summary>
      /// Called when <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/> is called.
      /// </summary>
      public override void OnApplyTemplate ()
      {
         base.OnApplyTemplate ();
         var textBox = GetTemplateChild ("PART_EditableTextBox") as TextBox;
         if (textBox != null)
            this.m_TextBox = textBox;
      }
   }
}
